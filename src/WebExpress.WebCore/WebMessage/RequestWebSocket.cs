using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebSession.Model;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a request for a WebSocket connection.
    /// </summary>
    public class RequestWebSocket : IRequest
    {
        private readonly ParameterDictionary _param = [];

        /// <summary>
        /// The context of the web server.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; protected set; }

        /// <summary>
        /// Returns the request method (typically GET for WebSocket handshake).
        /// </summary>
        public RequestMethod Method { get; private set; }

        /// <summary>
        /// Returns the URI.
        /// </summary>
        public UriEndpoint Uri { get; set; }

        /// <summary>
        /// Returns the session.
        /// </summary>
        public Session Session { get; private set; }

        /// <summary>
        /// Returns the HTTP version.
        /// </summary>
        public string Protocoll { get; private set; }

        /// <summary>
        /// Returns the header fields.
        /// </summary>
        public RequestHeaderFields Header { get; private set; }

        /// <summary>
        /// Returns the server's local endpoint.
        /// </summary>
        public EndPoint LocalEndPoint { get; private set; }

        /// <summary>
        /// Returns the client's remote endpoint.
        /// </summary>
        public EndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// Indicates whether the connection is secured (wss).
        /// </summary>
        public bool IsSecureConnection { get; private set; }

        /// <summary>
        /// Returns the scheme (ws or wss).
        /// </summary>
        public UriScheme Scheme { get; private set; }

        /// <summary>
        /// Returns the request identifier.
        /// </summary>
        public string RequestTraceIdentifier { get; private set; }

        /// <summary>
        /// Returns the culture.
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                try
                {
                    var languages = Header?.AcceptLanguage?.FirstOrDefault();
                    var language = languages?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
                    return new CultureInfo(language);
                }
                catch
                {
                    return HttpServerContext.Culture ?? CultureInfo.CurrentCulture;
                }
            }
        }

        /// <summary>
        /// Returns the current WebSocket message type.
        /// </summary>
        public string WebSocketMessageType { get; internal set; }

        /// <summary>
        /// Returns true, if the WebSocket is open.
        /// </summary>
        public bool IsWebSocketOpen { get; internal set; }

        /// <summary>
        /// Initializes a new instance for a WebSocket request. 
        /// Use this after WebSocket handshake is established.
        /// </summary>
        /// <param name="contextFeatures">The feature collection from ASP.NET Core.</param>
        /// <param name="header">The parsed header fields of the request.</param>
        /// <param name="httpServerContext">The context of the web server.</param>
        internal RequestWebSocket(IFeatureCollection contextFeatures, RequestHeaderFields header, IHttpServerContext httpServerContext)
        {
            HttpServerContext = httpServerContext;
            Header = header;

            var connectionFeature = contextFeatures.Get<IHttpConnectionFeature>();
            var requestFeature = contextFeatures.Get<IHttpRequestFeature>();

            Method = RequestMethod.GET; // WebSocket handshake always uses GET
            Protocoll = requestFeature.Protocol;

            Scheme = requestFeature.Scheme.Equals("wss", StringComparison.OrdinalIgnoreCase) ? UriScheme.Wss :
                requestFeature.Scheme.Equals("ws", StringComparison.OrdinalIgnoreCase) ? UriScheme.Ws : UriScheme.Http;

            LocalEndPoint = new IPEndPoint(connectionFeature.LocalIpAddress, connectionFeature.LocalPort);
            RemoteEndPoint = new IPEndPoint(connectionFeature.RemoteIpAddress, connectionFeature.RemotePort);
            RequestTraceIdentifier = connectionFeature.ConnectionId;
            IsSecureConnection = Scheme == UriScheme.Wss;

            // build the uri-endpoint for WebSocket (assume raw target is path + query)
            Uri = new UriEndpoint
             (
                 Scheme,
                 new UriAuthority()
                 {
                     Host = Header.Host,
                     Port = connectionFeature.LocalPort
                 },
                 requestFeature.RawTarget
             );

            // WebSocket specific defaults
            WebSocketMessageType = null;
            IsWebSocketOpen = true;

            ParseSessionParams();
        }

        /// <summary>
        /// Adds a collection of parameters to the current instance.
        /// </summary>
        /// <param name="param">
        /// An enumerable collection of <see cref="Parameter"/> objects to add. Cannot be null.
        /// </param>
        public void AddParameter(IEnumerable<Parameter> param)
        {
            foreach (var p in param)
            {
                AddParameter(p);
            }
        }

        /// <summary>
        /// Adds a parameter to the collection, replacing any existing parameter with the 
        /// same key (case-insensitive).
        /// </summary>
        /// <param name="param">
        /// The parameter to add to the collection. Cannot be null. The parameter's key 
        /// is used as the unique identifier.
        /// </param>
        public void AddParameter(Parameter param)
        {
            var key = param.Key.ToLower();

            if (!_param.TryAdd(key, param))
            {
                _param[key] = param;
            }
        }

        /// <summary>
        /// Retrieves the parameter with the specified name, if it exists.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter to retrieve. Cannot be null, empty, or consist 
        /// only of white-space characters. The comparison is case-insensitive.
        /// </param>
        /// <returns>
        /// The parameter associated with the specified name, or null if no such parameter exists.
        /// </returns>
        public IParameter GetParameter(string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && HasParameter(name))
            {
                return _param[name.ToLower()];
            }

            return null;
        }

        /// <summary>
        /// Retrieves the parameter of the specified type from the current parameter 
        /// collection, if it exists.
        /// </summary>
        /// <typeparam name="TParameter">
        /// The type of parameter to retrieve. Must implement the IParameter interface.
        /// </typeparam>
        /// <returns>
        /// An instance of the specified parameter type with its value and scope set
        /// if the parameter exists; otherwise, null.
        /// </returns>
        public IParameter GetParameter<TParameter>()
            where TParameter : IParameter
        {
            var parameter = Parameter.GetParameter<TParameter>();
            if
            (
                parameter is not null
                && !string.IsNullOrWhiteSpace(parameter.Key)
                && HasParameter(parameter.Key)
            )
            {
                var p = _param[parameter.Key.ToLower()];
                parameter.Value = p.Value;
                parameter.Scope = p.Scope;

                return parameter;
            }

            return null;
        }

        /// <summary>
        /// Determines whether a parameter with the specified name exists.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter to locate. The comparison is case-insensitive. Can be null.
        /// </param>
        /// <returns>
        /// True if a parameter with the specified name exists; otherwise, false.
        /// </returns>
        public bool HasParameter(string name)
        {
            if (name is null)
            {
                return false;
            }

            return _param.ContainsKey(name.ToLower());
        }

        /// <summary>
        /// Parses session parameters from the current session and adds them to 
        /// the parameter collection.
        /// </summary>
        private void ParseSessionParams()
        {
            Session = WebEx.ComponentHub?.SessionManager?.GetSession(this);

            var property = Session?.GetProperty<SessionPropertyParameter>();
            if (property is not null && property.Params is not null)
            {
                foreach (var param in property.Params)
                {
                    AddParameter(new Parameter(param.Key?.ToLower(), param.Value.Value, ParameterScope.Session));
                }
            }
        }
    }
}