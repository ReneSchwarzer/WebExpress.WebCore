using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebSocket.Model;
using WebExpress.WebCore.WebSocket.Protocol;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// The socket manager manages socket endpoints (see RFC 6455 – The WebSocket Protocol) which can be called with a URI.
    /// </summary>
    public class SocketManager : ISocketManager
    {
        private const string _webSocketGuid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly SocketDictionary _dictionary = new();

        /// <summary>
        /// An event that fires when a socket context is added.
        /// </summary>
        public event EventHandler<ISocketContext> AddSocket;

        /// <summary>
        /// An event that fires when a socket context is removed.
        /// </summary>
        public event EventHandler<ISocketContext> RemoveSocket;

        /// <summary>
        /// Returns all socket contexts.
        /// </summary>
        public IEnumerable<ISocketContext> Sockets => _dictionary.All;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used via Reflection.")]
        private SocketManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;

            var endpointRegistration = new EndpointRegistration()
            {
                EndpointResolver = (type, applicationContext) => applicationContext is not null
                    ? GetSockets(type, applicationContext)
                    : GetSockets(type),
                EndpointsResolver = () => Sockets
            };

            AddSocket += (sender, e) => endpointRegistration.AddEndpoint?.Invoke(sender, e);
            RemoveSocket += (sender, e) => endpointRegistration.RemoveEndpoint?.Invoke(sender, e);

            _componentHub.EndpointManager.Register<SocketContext>(endpointRegistration);

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress.webcore:socketmanager.initialization")
            );
        }

        /// <summary> 
        /// Handles the complete lifecycle of a WebSocket connection for the given HTTP context. 
        /// Performs the server-side upgrade, invokes connection callbacks, processes incoming 
        /// WebSocket frames, parses messages, dispatches them to the socket handler, and triggers 
        /// disconnect and error callbacks as required. 
        /// </summary> 
        /// <param name="httpContext"> 
        /// The current HTTP context containing the WebSocket upgrade request and connection metadata. 
        /// </param> 
        /// <param name="socketContext"> 
        /// Context information for the WebSocket endpoint, including supported subprotocols 
        /// and application-level metadata. 
        /// </param> 
        /// <returns> 
        /// A task that represents the asynchronous handling of the WebSocket connection. 
        /// </returns>
        public async Task HandleConnectionAsync(IHttpContext httpContext, ISocketContext socketContext)
        {
            var connectionId = Guid.NewGuid().ToString();
            var closeDescription = "closing";
            var cancellationToken = CancellationToken.None;
            var requestFeature = httpContext.Features.Get<IHttpRequestFeature>();
            var connection = httpContext.Request.Header.Connection;
            var upgrade = httpContext.Request.Header.Upgrade;
            var key = httpContext.Request.Header.SecWebSocketKey;

            // 1. perform handshake
            var responseSender = new ResponseSender();
            var response101 = new ResponseSwitchingProtocols
            (
                connection,
                upgrade,
                ComputeWebSocketAcceptKey(key)
            );

            await responseSender.SendAsync(httpContext, response101, true);

            // 2. create native web socket connection using the raw body stream
            var webSocket = new Socket(requestFeature.Body, cancellationToken);

            // 3. create ISocket instance
            var instance = await CreateSocketInstance(socketContext, webSocket);

            // 4. notify user code
            try
            {
                await instance.OnConnectedAsync();
            }
            catch
            {
                // optional
            }

            try
            {
                // 5. receive loop
                await webSocket.StartAsync();
            }
            catch (Exception ex)
            {
                closeDescription = "transport error";
                await instance.OnErrorAsync(ex);
            }

            var closeInfo = new SocketCloseInfo(SocketCloseStatus.NormalClosure, closeDescription);

            // 6. disconnect
            await instance.OnDisconnectedAsync(closeInfo);
        }

        /// <summary>
        /// Returns an enumeration of all socket contexts provided by a plugin.
        /// </summary>
        /// <param name="pluginContext">
        /// A context of a plugin whose sockets are to be returned.
        /// </param>
        /// <returns>An enumeration of socket contexts.</returns>
        public IEnumerable<ISocketContext> GetSockets(IPluginContext pluginContext)
        {
            return _dictionary.GetSockets(pluginContext);
        }

        /// <summary>
        /// Returns an enumeration of socket contexts filtered by endpoint type.
        /// </summary>
        /// <typeparam name="TSocket">The socket endpoint type.</typeparam>
        /// <returns>An enumeration of socket contexts.</returns>
        public IEnumerable<ISocketContext> GetSockets<TSocket>()
            where TSocket : ISocket
        {
            return GetSockets(typeof(TSocket));
        }

        /// <summary>
        /// Returns an enumeration of socket contexts filtered by endpoint type.
        /// </summary>
        /// <param name="socketType">The socket endpoint type.</param>
        /// <returns>An enumeration of socket contexts.</returns>
        public IEnumerable<ISocketContext> GetSockets(Type socketType)
        {
            return _dictionary.GetSockets(socketType);
        }

        /// <summary>
        /// Returns an enumeration of socket contexts filtered by endpoint type 
        /// and application context.
        /// </summary>
        /// <param name="socketType">The socket endpoint type.</param>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of socket contexts.</returns>
        public IEnumerable<ISocketContext> GetSockets(Type socketType, IApplicationContext applicationContext)
        {
            return _dictionary.GetSockets(socketType, applicationContext);
        }

        /// <summary>
        /// Returns an enumeration of socket contexts filtered by endpoint type and 
        /// application context.
        /// </summary>
        /// <typeparam name="TSocket">The socket endpoint type.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of socket contexts.</returns>
        public IEnumerable<ISocketContext> GetSockets<TSocket>(IApplicationContext applicationContext) where TSocket : ISocket
        {
            return _dictionary.GetSockets<TSocket>(applicationContext);
        }

        /// <summary>
        /// Returns the socket context by application context and socket id.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="socketId">The socket id.</param>
        /// <returns>A socket context or null.</returns>
        public ISocketContext GetSocket(IApplicationContext applicationContext, string socketId)
        {
            return _dictionary.GetSocket(applicationContext, socketId);
        }

        /// <summary>
        /// Returns the socket context by application id and socket id.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="socketId">The socket id.</param>
        /// <returns>A socket context or null.</returns>
        public ISocketContext GetSocket(string applicationId, string socketId)
        {
            return _dictionary.GetSocket(applicationId, socketId);
        }

        /// <summary>
        /// Creates a new socket endpoint instance and returns it. 
        /// If an instance is cached, the cached instance is returned.
        /// </summary>
        /// <param name="socketContext">The context used for socket creation.</param>
        /// <param name="webSocket">The accepted native WebSocket connection.</param>
        /// <returns>The created or cached endpoint instance.</returns>
        private async Task<ISocket> CreateSocketInstance
        (
            ISocketContext socketContext,
            Socket webSocket
        )
        {
            var resourceItem = _dictionary.GetSocketItem(socketContext);

            if (resourceItem is not null && resourceItem.Instance is null)
            {
                await using var stream = new SocketWriteStream(webSocket, socketContext.MessageType);

                var instance = ComponentActivator.CreateInstance<ISocket, ISocketContext>
                (
                    resourceItem.SocketClass,
                    socketContext,
                    _httpServerContext,
                    _componentHub,
                    socketContext.ApplicationContext,
                    stream as ISocketWriteStream
                );

                if (resourceItem.Cache)
                {
                    resourceItem.Instance = instance;
                }

                return instance;
            }

            return resourceItem?.Instance;
        }

        /// <summary>
        /// Discovers and binds sockets to an application.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin whose sockets are to be associated.</param>
        private void Register(IPluginContext pluginContext)
        {
            if (_dictionary.Contains(pluginContext))
            {
                return;
            }

            Register(pluginContext, _componentHub.ApplicationManager.GetApplications(pluginContext));
        }

        /// <summary>
        /// Discovers and binds sockets to an application.
        /// </summary>
        /// <param name="applicationContext">The context of the application whose sockets are to be associated.</param>
        private void Register(IApplicationContext applicationContext)
        {
            foreach (var pluginContext in _componentHub.PluginManager.GetPlugins(applicationContext))
            {
                if (_dictionary.Contains(pluginContext, applicationContext))
                {
                    continue;
                }

                Register(pluginContext, [applicationContext]);
            }
        }

        /// <summary>
        /// Registers sockets for a given plugin and application contexts.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContexts">The application context(s).</param>
        private void Register(IPluginContext pluginContext, IEnumerable<IApplicationContext> applicationContexts)
        {
            var assembly = pluginContext?.Assembly;

            foreach (var socketType in assembly.GetTypes()
                .Where(x => x.IsClass == true && x.IsSealed && x.IsPublic)
                .Where(x => x.GetInterfaces().Where(i => i == typeof(ISocket)).Any()))
            {
                var id = socketType.FullName?.ToLower();
                var includeSubPaths = false;
                var conditions = new List<ICondition>();
                var cache = false;
                var subProtocols = new List<string>();
                var messageType = SocketMessageType.Text;
                var maxMessageSize = ulong.MinValue;
                var attributes = socketType.CustomAttributes
                    .Where(x => !x.AttributeType.GetInterfaces().Contains(typeof(IEndpointAttribute)));

                foreach (var customAttribute in socketType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IEndpointAttribute))))
                {
                    if (customAttribute.AttributeType == typeof(IncludeSubPathsAttribute))
                    {
                        includeSubPaths = Convert.ToBoolean(customAttribute.ConstructorArguments.FirstOrDefault().Value);
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ConditionAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ConditionAttribute<>).Namespace)
                    {
                        var condition = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                        conditions.Add(Activator.CreateInstance(condition) as ICondition);
                    }
                    else if (customAttribute.AttributeType == typeof(CacheAttribute))
                    {
                        cache = true;
                    }
                }

                foreach (var customAttribute in socketType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(ISocketAttribute))))
                {
                    if (customAttribute.AttributeType == typeof(MessageTypeAttribute))
                    {
                        messageType = Enum.Parse<SocketMessageType>(customAttribute.ConstructorArguments.FirstOrDefault().Value.ToString());
                    }
                    else if (customAttribute.AttributeType == typeof(SubProtocolAttribute))
                    {
                        subProtocols.Add(customAttribute.ConstructorArguments.FirstOrDefault().Value.ToString());
                    }
                    else if (customAttribute.AttributeType == typeof(MaxMessageSizeAttribute))
                    {
                        maxMessageSize = Convert.ToUInt64(customAttribute.ConstructorArguments.FirstOrDefault().Value.ToString());
                    }
                }

                // assign the socket to existing applications
                foreach (var applicationContext in applicationContexts)
                {
                    var prefix = applicationContext.Route.Concat
                    (
                        applicationContext.PluginContext != pluginContext
                            ? pluginContext.PluginName.ToLower()
                            : ""
                    );
                    var routePath = EndpointManager.CreateEndpointRoute(socketType, prefix, null);
                    var socketContext = new SocketContext()
                    {
                        EndpointId = new ComponentId(id),
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        Route = routePath,
                        MessageType = messageType,
                        SupportedSubProtocols = subProtocols,
                        MaxMessageSize = maxMessageSize,
                        Cache = cache,
                        Conditions = conditions,
                        IncludeSubPaths = includeSubPaths,
                        Attributes = EndpointManager.GetAttributeInstances(attributes)
                    };

                    var socketItem = new SocketItem(_componentHub.EndpointManager)
                    {
                        EndpointId = new ComponentId(id),
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        SocketContext = socketContext,
                        SocketClass = socketType,
                        MessageType = messageType,
                        SupportedSubProtocols = subProtocols,
                        MaxMessageSize = maxMessageSize,
                        Cache = cache,
                        Conditions = conditions,
                        Attributes = attributes.Select(x => x.AttributeType)
                    };

                    if (_dictionary.AddSocketItem(pluginContext, applicationContext, socketItem))
                    {
                        OnAddSocket(socketItem.SocketContext);

                        _httpServerContext?.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress.webcore:socketmanager.addsocket",
                                id,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Removes all sockets associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the sockets to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            if (pluginContext is null)
            {
                return;
            }

            // deregister all sockets associated with this plugin context
            foreach (var socketContext in _dictionary.RemoveSocket(pluginContext))
            {
                OnRemoveSocket(socketContext);

                _httpServerContext?.Log.Debug
                (
                    I18N.Translate
                    (
                        "webexpress.webcore:socketmanager.removesocket",
                        socketContext.EndpointId,
                        socketContext.ApplicationContext.ApplicationId
                    )
                );
            }
        }

        /// <summary>
        /// Removes all sockets associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">
        /// The context of the application that contains the sockets to remove.
        /// </param>
        internal void Remove(IApplicationContext applicationContext)
        {
            if (applicationContext is null)
            {
                return;
            }

            // deregister all sockets associated with this application context
            foreach (var socketContext in _dictionary.RemoveSocket(applicationContext))
            {
                OnRemoveSocket(socketContext);

                _httpServerContext?.Log.Debug
                (
                    I18N.Translate
                    (
                        "webexpress.webcore:socketmanager.removesocket",
                        socketContext.EndpointId,
                        socketContext.ApplicationContext.ApplicationId
                    )
                );
            }
        }

        /// <summary>
        /// Raises the AddSocket event.
        /// </summary>
        /// <param name="socketContext">The socket context.</param>
        private void OnAddSocket(ISocketContext socketContext)
        {
            AddSocket?.Invoke(this, socketContext);
        }

        /// <summary>
        /// Raises the RemoveSocket event.
        /// </summary>
        /// <param name="socketContext">The socket context.</param>
        private void OnRemoveSocket(ISocketContext socketContext)
        {
            RemoveSocket?.Invoke(this, socketContext);
        }

        /// <summary>
        /// Raises the event when a plugin is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="pluginContext">The context of the plugin being added.</param>
        private void OnAddPlugin(object sender, IPluginContext pluginContext)
        {
            Register(pluginContext);
        }

        /// <summary>  
        /// Raises the event when a plugin is removed.  
        /// </summary>  
        /// <param name="sender">The source of the event.</param>  
        /// <param name="pluginContext">The context of the plugin being removed.</param>  
        private void OnRemovePlugin(object sender, IPluginContext pluginContext)
        {
            Remove(pluginContext);
        }

        /// <summary>
        /// Raises the event when an application is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="applicationContext">The context of the application being added.</param>
        private void OnAddApplication(object sender, IApplicationContext applicationContext)
        {
            Register(applicationContext);
        }

        /// <summary>
        /// Raises the event when an application is removed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="applicationContext">The application context.</param>
        private void OnRemoveApplication(object sender, IApplicationContext applicationContext)
        {
            Remove(applicationContext);
        }

        /// <summary> 
        /// Dispatches the parsed <see cref="ISocketMessage"/> to the socket handler implementation. 
        /// Invokes the receive callback and forwards any handler exceptions to the error callback. 
        /// </summary> 
        /// <param name="instance"> 
        /// The <see cref="ISocket"/> implementation responsible for processing the message. 
        /// </param> 
        /// <param name="message"> 
        /// The message to be delivered to the socket handler. 
        /// </param> 
        /// <returns> 
        /// A task that represents the asynchronous dispatch operation. 
        /// </returns>
        /// <summary>
        /// Dispatches a parsed <see cref="ISocketMessage"/> to the socket handler.
        /// Invokes the receive callback and forwards any handler exceptions to the error callback.
        /// </summary>
        private static async Task DispatchMessage(ISocket instance, ISocketMessage message)
        {
            if (instance == null || message == null)
            {
                return;
            }

            try
            {
                await instance.OnReceiveAsync(message);
            }
            catch (Exception ex)
            {
                await instance.OnErrorAsync(ex);
            }
        }

        /// <summary>
        /// Computes the Sec-WebSocket-Accept header value from the client key.
        /// </summary>
        /// <param name="key">The Sec-WebSocket-Key from the client.</param>
        /// <returns>The computed Sec-WebSocket-Accept value.</returns>
        private static string ComputeWebSocketAcceptKey(string key)
        {
            var combined = key + _webSocketGuid;
            var hash = SHA1.HashData(Encoding.UTF8.GetBytes(combined));

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
            _componentHub.PluginManager.AddPlugin -= OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin -= OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication -= OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication -= OnRemoveApplication;

            GC.SuppressFinalize(this);
        }
    }
}