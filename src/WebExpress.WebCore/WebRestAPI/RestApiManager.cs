using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    public class RestApiManager : IRestApiManager
    {
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly RestApiDictionary _dictionary = [];
        private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

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
        private RestApiManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;

            _componentHub.EndpointManager.Register<RestApiContext>
            (
                new EndpointRegistration()
                {
                    EndpointResolver = (type, applicationContext) => applicationContext != null ? GetRestApi(type, applicationContext) : GetRestApi(type),
                    EndpointsResolver = () => RestApis,
                    HandleRequest = (request, endpointContext) =>
                    {
                        var restApiContext = endpointContext as IRestApiContext;
                        var restApi = CreatePageInstance(restApiContext, request.Culture) as IRestApi;

                        if (restApiContext.Methods.Any(x => x.Equals((CrudMethod)request.Method)))
                        {
                            switch (request.Method)
                            {
                                case RequestMethod.POST:
                                    restApi.CreateData(request);

                                    return new ResponseOK();
                                case RequestMethod.GET:
                                    var data = restApi.GetData(request);
                                    if (data != null)
                                    {
                                        var jsonData = JsonSerializer.Serialize(data, _jsonOptions);
                                        var content = Encoding.UTF8.GetBytes(jsonData);

                                        return new ResponseOK
                                        {
                                            Content = content
                                        };
                                    }

                                    return new ResponseOK();
                                case RequestMethod.PATCH:
                                    restApi.UpdateData(request);

                                    return new ResponseOK();
                                case RequestMethod.DELETE:
                                    restApi.DeleteData(request);

                                    return new ResponseOK();
                            }
                        }

                        return new ResponseBadRequest()
                        {
                            Content = I18N.Translate("webexpress:restapimanager.methodnotsupported", request.Method.ToString())
                        };
                    }
                }
            );

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress:restapimanager.initialization")
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
        /// <param name="pageContext">The context used for rest api resource creation.</param>
        /// <param name="culture">The culture with the language settings.</param>
        /// <returns>The created or cached rest api resource.</returns>
        private IRestApi CreatePageInstance(IRestApiContext pageContext, CultureInfo culture)
        {
            var resourceItem = _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .FirstOrDefault(x => x.RestApiContext.Equals(pageContext));

            if (resourceItem != null && resourceItem.Instance == null)
            {
                var instance = ComponentActivator.CreateInstance<IRestApi, IRestApiContext>(resourceItem.RestApiClass, pageContext, _componentHub);

                if (instance is II18N i18n)
                {
                    i18n.Culture = culture;
                }

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
        /// <param name="applicationContext">The application context (optional).</param>
        private void Register(IPluginContext pluginContext, IEnumerable<IApplicationContext> applicationContexts)
        {
            var assembly = pluginContext?.Assembly;

            foreach (var resourceType in assembly.GetTypes()
                .Where(x => x.IsClass == true && x.IsSealed && x.IsPublic)
                .Where(x => x.GetInterface(typeof(IRestApi).Name) != null))
            {
                var id = resourceType.FullName?.ToLower();
                var segment = default(ISegmentAttribute);
                var title = resourceType.Name;
                var parent = default(Type);
                var contextPath = string.Empty;
                var includeSubPaths = false;
                var conditions = new List<ICondition>();
                var cache = false;
                var methods = new List<CrudMethod>();
                var version = 1u;

                foreach (var customAttribute in resourceType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IEndpointAttribute))))
                {
                    if (customAttribute.AttributeType.GetInterfaces().Contains(typeof(ISegmentAttribute)))
                    {
                        segment = resourceType.GetCustomAttributes(customAttribute.AttributeType, false).FirstOrDefault() as ISegmentAttribute;
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ParentAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ParentAttribute<>).Namespace)
                    {
                        parent = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                    }
                    else if (customAttribute.AttributeType == typeof(ContextPathAttribute))
                    {
                        contextPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(IncludeSubPathsAttribute))
                    {
                        includeSubPaths = Convert.ToBoolean(customAttribute.ConstructorArguments.FirstOrDefault().Value);
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ConditionAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ConditionAttribute<>).Namespace)
                    {
                        var condition = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                        conditions.Add(Activator.CreateInstance(condition) as ICondition);
                    }
                    else if (customAttribute.AttributeType.Name == typeof(MethodAttribute).Name && customAttribute.AttributeType.Namespace == typeof(MethodAttribute).Namespace)
                    {
                        var method = (CrudMethod)customAttribute.ConstructorArguments.FirstOrDefault().Value;
                        methods.Add(method);
                    }
                    else if (customAttribute.AttributeType == typeof(CacheAttribute))
                    {
                        cache = true;
                    }
                }

                foreach (var customAttribute in resourceType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IRestApiAttribute))))
                {
                    if (customAttribute.AttributeType.Name == typeof(VersionAttribute).Name && customAttribute.AttributeType.Namespace == typeof(VersionAttribute).Namespace)
                    {
                        version = Convert.ToUInt32(customAttribute.ConstructorArguments.FirstOrDefault().Value);
                    }

                }

                // assign the job to existing applications
                foreach (var applicationContext in _componentHub.ApplicationManager.GetApplications(pluginContext))
                {
                    var restApiContext = new RestApiContext(_componentHub.EndpointManager, parent, new UriResource(contextPath), segment.ToPathSegment())
                    {
                        EndpointId = resourceType.FullName.ToLower(),
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        Cache = cache,
                        Conditions = conditions,
                        Methods = methods,
                        Version = version,
                        IncludeSubPaths = includeSubPaths
                    };

                    var restApiItem = new RestApiItem(_componentHub.RestApiManager)
                    {
                        ParentType = parent,
                        RestApiContext = restApiContext,
                        RestApiClass = resourceType,
                        Methods = methods.Distinct(),
                        Version = version,
                        Cache = cache,
                        Conditions = conditions,
                        ContextPath = new UriResource(contextPath),
                        IncludeSubPaths = includeSubPaths,
                        PathSegment = segment.ToPathSegment(),
                        Log = _httpServerContext?.Log
                    };

                    if (_dictionary.AddRestApiItem(pluginContext, applicationContext, restApiItem))
                    {
                        OnAddRestApi(restApiContext);

                        _httpServerContext?.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress:restapimanager.addresource",
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
            if (!_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            foreach (var resourceItem in _dictionary[pluginContext].Values
                .SelectMany(x => x.Values))
            {
                OnRemoveRestApi(resourceItem.RestApiContext);
                resourceItem.Dispose();
            }

            _dictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Removes all jobs associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the jobs to remove.</param>
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
        /// Returns an enumeration of all containing rest api resource items of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose rest api resources are to be registered.</param>
        /// <returns>An enumeration of rest api resource items.</returns>
        private IEnumerable<RestApiItem> GetRestApiItems(IPluginContext pluginContext)
        {
            if (_dictionary.TryGetValue(pluginContext, out var pluginResources))
            {
                return pluginResources
                    .SelectMany(x => x.Value)
                    .Select(x => x.Value);
            }

            return [];
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
        /// Handles the event when an plugin is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the plugin being added.</param>
        private void OnAddPlugin(object sender, IPluginContext e)
        {
            Register(e);
        }

        /// <summary>  
        /// Handles the event when a plugin is removed.  
        /// </summary>  
        /// <param name="sender">The source of the event.</param>  
        /// <param name="e">The context of the plugin being removed.</param>  
        private void OnRemovePlugin(object sender, IPluginContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Handles the event when an application is removed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being removed.</param>
        private void OnRemoveApplication(object sender, IApplicationContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Handles the event when an application is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being added.</param>
        private void OnAddApplication(object sender, IApplicationContext e)
        {
            Register(e);
        }

        /// <summary>
        /// Collects and prepares information about the component for output in the log.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The depth of the log.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
            foreach (var resourcenItem in GetRestApiItems(pluginContext))
            {
                output.Add
                (
                    string.Empty.PadRight(deep) +
                    I18N.Translate
                    (
                        "webexpress:restapimanager.resource",
                        resourcenItem?.RestApiContext?.EndpointId,
                        string.Join(",", resourcenItem?.RestApiContext?.ApplicationContext?.ApplicationId)
                    )
                );
            }
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
        }
    }
}
