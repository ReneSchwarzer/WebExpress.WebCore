using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebRestApi.Model;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// The rest api manager manages rest api resources, which can be called with a URI (Uniform page Identifier).
    /// </summary>
    public partial class RestApiManager : IRestApiManager
    {
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly RestApiDictionary _dictionary = [];

        [GeneratedRegex(@"\.(?:_|V|v)(\d+)\.")]
        private static partial Regex ApiVersionRegex();

        /// <summary>
        /// An event that fires when an rest api resource is added.
        /// </summary>
        public event EventHandler<IRestApiContext> AddRestApi;

        /// <summary>
        /// An event that fires when an rest api resource is removed.
        /// </summary>
        public event EventHandler<IRestApiContext> RemoveRestApi;

        /// <summary>
        /// Returns all rest api resource contexts.
        /// </summary>
        public IEnumerable<IRestApiContext> RestApis => _dictionary.Values
            .SelectMany(x => x.Values)
            .SelectMany(x => x.Values)
            .Select(x => x.RestApiContext);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used via Reflection.")]
        private RestApiManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;

            var endpointtRegistration = new EndpointRegistration()
            {
                EndpointResolver = (type, applicationContext) => applicationContext != null ? GetRestApi(type, applicationContext) : GetRestApi(type),
                EndpointsResolver = () => RestApis,
                HandleRequest = (request, endpointContext) =>
                {
                    var restApiContext = endpointContext as IRestApiContext;
                    var restApi = CreateApiInstance(restApiContext) as IRestApi;

                    if (restApiContext.Methods.Any(x => x.Equals((CrudMethod)request.Method)))
                    {
                        switch (request.Method)
                        {
                            case RequestMethod.POST:
                                return restApi.CreateData(request) ?? new ResponseOK();
                            case RequestMethod.GET:
                                return restApi.GetData(request) ?? new ResponseOK();
                            case RequestMethod.PATCH:
                                return restApi.UpdateData(request) ?? new ResponseOK();
                            case RequestMethod.PUT:
                                return restApi.UpdateData(request) ?? new ResponseOK();
                            case RequestMethod.DELETE:
                                return restApi.DeleteData(request) ?? new ResponseOK();
                        }
                    }

                    return new ResponseBadRequest()
                    {
                        Content = I18N.Translate("webexpress.webcore:restapimanager.methodnotsupported", request.Method.ToString())
                    };
                }
            };

            AddRestApi += (sender, e) => endpointtRegistration.AddEndpoint?.Invoke(sender, e);
            RemoveRestApi += (sender, e) => endpointtRegistration.RemoveEndpoint?.Invoke(sender, e);

            _componentHub.EndpointManager.Register<RestApiContext>(endpointtRegistration);

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress.webcore:restapimanager.initialization")
            );
        }

        /// <summary>
        /// Returns an enumeration of all containing page contexts of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose pages are to be registered.</param>
        /// <returns>An enumeration of rest api resource contexts.</returns>
        public IEnumerable<IRestApiContext> GetRestApi(IPluginContext pluginContext)
        {
            if (_dictionary.TryGetValue(pluginContext, out var pluginResources))
            {
                return pluginResources
                    .SelectMany(x => x.Value)
                    .Select(x => x.Value.RestApiContext);
            }

            return [];
        }

        /// <summary>
        /// Returns an enumeration of rest api resource contextes.
        /// </summary>
        /// <typeparam name="T">The rest api resource type.</typeparam>
        /// <returns>An enumeration of rest api resource contextes.</returns>
        public IEnumerable<IRestApiContext> GetRestApi<T>() where T : IRestApi
        {
            return GetRestApi(typeof(T));
        }

        /// <summary>
        /// Returns an enumeration of rest api resource contextes.
        /// </summary>
        /// <param name="restApiType">The rest api resource type.</param>
        /// <returns>An enumeration of rest api resource contextes.</returns>
        public IEnumerable<IRestApiContext> GetRestApi(Type restApiType)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.RestApiClass.Equals(restApiType))
                .Select(x => x.RestApiContext);
        }

        /// <summary>
        /// Returns an enumeration of rest api resource contextes.
        /// </summary>
        /// <param name="restApiType">The page type.</param>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of page contextes.</returns>
        public IEnumerable<IRestApiContext> GetRestApi(Type restApiType, IApplicationContext applicationContext)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.RestApiClass.Equals(restApiType))
                .Where(x => x.RestApiContext.ApplicationContext.Equals(applicationContext))
                .Select(x => x.RestApiContext);
        }

        /// <summary>
        /// Returns an enumeration of rest api resource contextes.
        /// </summary>
        /// <typeparam name="T">The rest api resource type.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of rest api resource contextes.</returns>
        public IEnumerable<IRestApiContext> GetRestApi<T>(IApplicationContext applicationContext) where T : IRestApi
        {
            return _dictionary.Values
                 .SelectMany(x => x.Values)
                 .SelectMany(x => x.Values)
                 .Where(x => x.RestApiClass.Equals(typeof(T)))
                 .Where(x => x.RestApiContext.ApplicationContext.Equals(applicationContext))
                 .Select(x => x.RestApiContext);
        }

        /// <summary>
        /// Returns the rest api resource context.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="restApiId">The rest api resource id.</param>
        /// <returns>An rest api resource context or null.</returns>
        public IRestApiContext GetRestApi(IApplicationContext applicationContext, string restApiId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.RestApiContext.ApplicationContext.Equals(applicationContext))
                .Where(x => x.RestApiContext.EndpointId.Equals(restApiId))
                .Select(x => x.RestApiContext)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns the rest api resource context.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="restApiId">The rest api resource id.</param>
        /// <returns>An rest api resource context or null.</returns>
        public IRestApiContext GetRestApi(string applicationId, string restApiId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.RestApiContext.ApplicationContext.ApplicationId.Equals(applicationId))
                .Where(x => x.RestApiContext.EndpointId.Equals(restApiId))
                .Select(x => x.RestApiContext)
                .FirstOrDefault();
        }

        /// <summary>
        /// Creates a new rest api resource and returns it. If a rest api resource already exists (through caching), the existing instance is returned.
        /// </summary>
        /// <param name="apiContext">The context used for rest api resource creation.</param>
        /// <returns>The created or cached rest api resource.</returns>
        private IRestApi CreateApiInstance(IRestApiContext apiContext)
        {
            var resourceItem = _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .FirstOrDefault(x => x.RestApiContext.Equals(apiContext));

            if (resourceItem != null && resourceItem.Instance == null)
            {
                var instance = ComponentActivator.CreateInstance<IRestApi, IRestApiContext>
                (
                    resourceItem.RestApiClass,
                    apiContext,
                    _httpServerContext,
                    _componentHub,
                    apiContext.ApplicationContext
                );

                if (resourceItem.Cache)
                {
                    resourceItem.Instance = instance;
                }

                return instance;
            }

            return resourceItem?.Instance as IRestApi;
        }

        /// <summary>
        /// Discovers and binds rest apis to an application.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin whose rest apis are to be associated.</param>
        private void Register(IPluginContext pluginContext)
        {
            if (_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            Register(pluginContext, _componentHub.ApplicationManager.GetApplications(pluginContext));
        }

        /// <summary>
        /// Discovers and binds rest apis to an application.
        /// </summary>
        /// <param name="applicationContext">The context of the application whose rest apis are to be associated.</param>
        private void Register(IApplicationContext applicationContext)
        {
            foreach (var pluginContext in _componentHub.PluginManager.GetPlugins(applicationContext))
            {
                if (_dictionary.TryGetValue(pluginContext, out var appDict) && appDict.ContainsKey(applicationContext))
                {
                    continue;
                }

                Register(pluginContext, [applicationContext]);
            }
        }

        /// <summary>
        /// Registers rest apis for a given plugin and application context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContexts">The application context (optional).</param>
        private void Register(IPluginContext pluginContext, IEnumerable<IApplicationContext> applicationContexts)
        {
            var assembly = pluginContext?.Assembly;

            foreach (var restApiType in assembly.GetTypes()
                .Where(x => x.IsClass == true && x.IsSealed && x.IsPublic)
                .Where(x => x.GetInterface(typeof(IRestApi).Name) != null))
            {
                var id = restApiType.FullName?.ToLower();
                var segment = default(ISegmentAttribute);
                var title = restApiType.Name;
                var contextPath = string.Empty;
                var includeSubPaths = false;
                var conditions = new List<ICondition>();
                var cache = false;
                var methods = new List<CrudMethod>();
                var match = ApiVersionRegex().Match(id);
                var versionSegment = match.Success ? match.Groups[0].Value.Replace(".", "") : "";
                var version = match.Success && uint.TryParse(match.Groups[1].Value, out var result) ? result : 1u;
                var attributes = restApiType.CustomAttributes
                    .Where(x => !x.AttributeType.GetInterfaces().Contains(typeof(IEndpointAttribute)) &&
                    !x.AttributeType.GetInterfaces().Contains(typeof(IPageAttribute)));

                foreach (var customAttribute in restApiType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IEndpointAttribute))))
                {
                    if (customAttribute.AttributeType.GetInterfaces().Contains(typeof(ISegmentAttribute)))
                    {
                        segment = restApiType.GetCustomAttributes(customAttribute.AttributeType, false).FirstOrDefault() as ISegmentAttribute;
                    }
                    else if (customAttribute.AttributeType == typeof(ContextPathAttribute))
                    {
                        contextPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(IncludeSubPathsAttribute))
                    {
                        includeSubPaths = Convert.ToBoolean(customAttribute.ConstructorArguments.FirstOrDefault().Value);
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ConditionAttribute<>).Name
                        && customAttribute.AttributeType.Namespace == typeof(ConditionAttribute<>).Namespace)
                    {
                        var condition = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                        conditions.Add(Activator.CreateInstance(condition) as ICondition);
                    }
                    else if (customAttribute.AttributeType.Name == typeof(MethodAttribute).Name
                        && customAttribute.AttributeType.Namespace == typeof(MethodAttribute).Namespace)
                    {
                        var method = (CrudMethod)customAttribute.ConstructorArguments.FirstOrDefault().Value;
                        methods.Add(method);
                    }
                    else if (customAttribute.AttributeType == typeof(CacheAttribute))
                    {
                        cache = true;
                    }
                }

                // assign the rest api to existing applications
                foreach (var applicationContext in applicationContexts)
                {
                    var prefix = applicationContext.Route.Concat
                    (
                        applicationContext.PluginContext != pluginContext
                            ? pluginContext.PluginName.ToLower()
                            : ""
                    );

                    var routePath = EndpointManager.CreateEndpointRoute
                    (
                        restApiType,
                        prefix,
                        segment,
                        [new UriPathSegmentConstant("api"), new UriPathSegmentVariableInt($"{version}") { VariableName = "_apiVersion" }],
                        ["api", "restapi", "rest"]
                    ).RemoveSegment(versionSegment);

                    var restApiContext = new RestApiContext()
                    {
                        EndpointId = new ComponentId(id),
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        Route = routePath,
                        Cache = cache,
                        Conditions = conditions,
                        IncludeSubPaths = includeSubPaths,
                        Attributes = EndpointManager.GetAttributeInstances(attributes),
                        Version = version,
                        Methods = methods.Distinct()
                    };

                    var restApiItem = new RestApiItem(_componentHub.EndpointManager)
                    {
                        EndpointId = new ComponentId(restApiType.FullName),
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        RestApiContext = restApiContext,
                        RestApiClass = restApiType,
                        Methods = methods.Distinct(),
                        Version = version,
                        Cache = cache,
                        Conditions = conditions,
                        IncludeSubPaths = includeSubPaths,
                        Attributes = attributes.Select(x => x.AttributeType)
                    };

                    if (_dictionary.AddRestApiItem(pluginContext, applicationContext, restApiItem))
                    {
                        OnAddRestApi(restApiItem.RestApiContext);

                        _httpServerContext?.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress.webcore:restapimanager.addrestapi",
                                id,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Removes all pages associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the rest api resources to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            if (pluginContext == null)
            {
                return;
            }

            // the plugin has not been registered in the manager
            if (_dictionary.TryGetValue(pluginContext, out var value))
            {
                foreach (var resourceItem in value.Values
                    .SelectMany(x => x.Values))
                {
                    OnRemoveRestApi(resourceItem.RestApiContext);
                    resourceItem.Dispose();
                }

                _dictionary.Remove(pluginContext);
            }
        }

        /// <summary>
        /// Removes all events associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the events to remove.</param>
        internal void Remove(IApplicationContext applicationContext)
        {
            if (applicationContext == null)
            {
                return;
            }

            foreach (var pluginDict in _dictionary.Values)
            {
                foreach (var appDict in pluginDict.Where(x => x.Key == applicationContext).Select(x => x.Value))
                {
                    foreach (var resourceItem in appDict.Values)
                    {
                        OnRemoveRestApi(resourceItem.RestApiContext);
                        resourceItem.Dispose();
                    }
                }

                pluginDict.Remove(applicationContext);
            }
        }

        /// <summary>
        /// Raises the AddRestApi event.
        /// </summary>
        /// <param name="resourceContext">The rest api resource context.</param>
        private void OnAddRestApi(IRestApiContext resourceContext)
        {
            AddRestApi?.Invoke(this, resourceContext);
        }

        /// <summary>
        /// Raises the RemoveRestApi event.
        /// </summary>
        /// <param name="pageContext">The rest api resource context.</param>
        private void OnRemoveRestApi(IRestApiContext pageContext)
        {
            RemoveRestApi?.Invoke(this, pageContext);
        }

        /// <summary>
        /// Raises the event when an plugin is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the plugin being added.</param>
        private void OnAddPlugin(object sender, IPluginContext e)
        {
            Register(e);
        }

        /// <summary>  
        /// Raises the event when a plugin is removed.  
        /// </summary>  
        /// <param name="sender">The source of the event.</param>  
        /// <param name="e">The context of the plugin being removed.</param>  
        private void OnRemovePlugin(object sender, IPluginContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Raises the event when an application is removed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being removed.</param>
        private void OnRemoveApplication(object sender, IApplicationContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Raises the event when an application is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being added.</param>
        private void OnAddApplication(object sender, IApplicationContext e)
        {
            Register(e);
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
