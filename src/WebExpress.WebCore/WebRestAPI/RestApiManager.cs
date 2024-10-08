using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebRestApi.Model;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebCore.WebStatusPage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// The rest api manager manages rest api resources, which can be called with a URI (Uniform page Identifier).
    /// </summary>
    public class RestApiManager : IRestApiManager
    {
        private readonly IComponentHub _componentManager;
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
            .SelectMany(x => x.RestApiContexts);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentManager">The component manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private RestApiManager(IComponentHub componentManager, IHttpServerContext httpServerContext)
        {
            _componentManager = componentManager;

            _componentManager.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            _componentManager.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };

            _componentManager.ModuleManager.AddModule += (sender, moduleContext) =>
            {
                AssignToModule(moduleContext);
            };

            _componentManager.ModuleManager.RemoveModule += (sender, moduleContext) =>
            {
                DetachFromModule(moduleContext);
            };

            _componentManager.SitemapManager.Register<RestApiContext>
            (
                new EndpointRegistration()
                {
                    Factory = (resourceContext, uri, culture) => CreatePageInstance(resourceContext as IRestApiContext, culture),
                    ContextResolver = (type, moduleContext) => moduleContext != null ? GetRestApi(type, moduleContext) : GetRestApi(type),
                    EndpointResolver = () => RestApis,
                    HandleRequest = (endpoint, endpontContext, request) =>
                    {
                        var restApi = endpoint as IRestApi;
                        var restApiContext = endpontContext as IRestApiContext;

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
            if (!_dictionary.ContainsKey(pluginContext))
            {
                return [];
            }

            return _dictionary[pluginContext].Values
                .SelectMany(x => x.RestApiContexts);
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
                .SelectMany(x => x.RestApiContexts, (x, y) => new { x.RestApiClass, RestApiContext = y })
                .Where(x => x.RestApiClass.Equals(restApiType))
                .Select(x => x.RestApiContext);
        }

        /// <summary>
        /// Returns an enumeration of rest api resource contextes.
        /// </summary>
        /// <param name="restApiType">The page type.</param>
        /// <param name="moduleContext">The context of the module.</param>
        /// <returns>An enumeration of page contextes.</returns>
        public IEnumerable<IRestApiContext> GetRestApi(Type restApiType, IModuleContext moduleContext)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.RestApiContexts, (x, y) => new { x.RestApiClass, RestApiContext = y })
                .Where(x => x.RestApiContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.RestApiClass.Equals(restApiType))
                .Select(x => x.RestApiContext);
        }

        /// <summary>
        /// Returns an enumeration of rest api resource contextes.
        /// </summary>
        /// <typeparam name="T">The rest api resource type.</typeparam>
        /// <param name="moduleContext">The context of the module.</param>
        /// <returns>An enumeration of rest api resource contextes.</returns>
        public IEnumerable<IRestApiContext> GetRestApi<T>(IModuleContext moduleContext) where T : IRestApi
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.RestApiContexts, (x, y) => new { x.RestApiClass, RestApiContext = y })
                .Where(x => x.RestApiContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.RestApiClass.Equals(typeof(T)))
                .Select(x => x.RestApiContext);
        }

        /// <summary>
        /// Returns the rest api resource context.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        /// <param name="restApiId">The rest api resource id.</param>
        /// <returns>An rest api resource context or null.</returns>
        public IRestApiContext GetRestApi(IModuleContext moduleContext, string restApiId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.RestApiContexts, (x, y) => new { x.RestApiClass, RestApiContext = y })
                .Where(x => x.RestApiContext.ModuleContext?.ApplicationContext != null)
                .Where(x => x.RestApiContext.ModuleContext.ApplicationContext.ApplicationId.Equals(moduleContext.ApplicationContext.ApplicationId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.RestApiContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.RestApiContext.EndpointId.Equals(restApiId, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.RestApiContext)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns the rest api resource context.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        /// <param name="restApiType">The rest api resource type.</param>
        /// <returns>An rest api resource context or null.</returns>
        public IRestApiContext GetRestApi(IModuleContext moduleContext, Type restApiType)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.RestApiContexts, (x, y) => new { x.RestApiClass, RestApiContext = y })
                .Where(x => x.RestApiContext.ModuleContext?.ApplicationContext != null)
                .Where(x => x.RestApiContext.ModuleContext.ApplicationContext.ApplicationId.Equals(moduleContext.ApplicationContext.ApplicationId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.RestApiContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.RestApiClass.Equals(restApiType))
                .Select(x => x.RestApiContext)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns the rest api resource context.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="restApiId">The rest api resource id.</param>
        /// <returns>An rest api resource context or null.</returns>
        public IRestApiContext GetRestApi(string applicationId, string moduleId, string restApiId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.RestApiContexts)
                .Where(x => x.ModuleContext != null && x.ModuleContext.ApplicationContext != null)
                .Where(x => x.ModuleContext.ApplicationContext.ApplicationId.Equals(applicationId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.ModuleContext.ModuleId.Equals(moduleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.EndpointId.Equals(restApiId, StringComparison.OrdinalIgnoreCase))
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
                .FirstOrDefault(x => x.RestApiContexts.Contains(pageContext));

            if (resourceItem != null && resourceItem.Instance == null)
            {
                var instance = ComponentActivator.CreateInstance<IRestApi, IRestApiContext>(resourceItem.RestApiClass, pageContext, _componentManager);

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
        /// Discovers and registers rest api resources from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose rest api resources are to be registered.</param>
        public void Register(IPluginContext pluginContext)
        {
            if (_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            var assembly = pluginContext?.Assembly;

            _dictionary.Add(pluginContext, []);
            var dict = _dictionary[pluginContext];

            foreach (var resourceType in assembly.GetTypes()
                .Where(x => x.IsClass == true && x.IsSealed && x.IsPublic)
                .Where(x => x.GetInterface(typeof(IRestApi).Name) != null)
                .Where(x => x.GetInterface(typeof(IStatusPage).Name) == null))
            {
                var id = resourceType.FullName?.ToLower();
                var segment = default(ISegmentAttribute);
                var title = resourceType.Name;
                var parent = default(string);
                var contextPath = string.Empty;
                var includeSubPaths = false;
                var moduleId = string.Empty;
                var conditions = new List<ICondition>();
                var optional = false;
                var cache = false;
                var methods = new List<CrudMethod>();
                var version = 1u;

                foreach (var customAttribute in resourceType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IEndpointAttribute))))
                {
                    var buf = typeof(ModuleAttribute<>);

                    if (customAttribute.AttributeType.GetInterfaces().Contains(typeof(ISegmentAttribute)))
                    {
                        segment = resourceType.GetCustomAttributes(customAttribute.AttributeType, false).FirstOrDefault() as ISegmentAttribute;
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ParentAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ParentAttribute<>).Namespace)
                    {
                        parent = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower();
                    }
                    else if (customAttribute.AttributeType == typeof(ContextPathAttribute))
                    {
                        contextPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(IncludeSubPathsAttribute))
                    {
                        includeSubPaths = Convert.ToBoolean(customAttribute.ConstructorArguments.FirstOrDefault().Value);
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ModuleAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ModuleAttribute<>).Namespace)
                    {
                        moduleId = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower();
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
                    else if (customAttribute.AttributeType.Name == typeof(VersionAttribute).Name && customAttribute.AttributeType.Namespace == typeof(VersionAttribute).Namespace)
                    {
                        version = Convert.ToUInt32(customAttribute.ConstructorArguments.FirstOrDefault().Value);
                    }
                    else if (customAttribute.AttributeType == typeof(CacheAttribute))
                    {
                        cache = true;
                    }
                    else if (customAttribute.AttributeType == typeof(OptionalAttribute))
                    {
                        optional = true;
                    }
                }

                if (string.IsNullOrEmpty(moduleId))
                {
                    // no module specified
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate
                        (
                            "webexpress:restapimanager.moduleless",
                            id
                        )
                    );

                    continue;
                }

                if (!dict.ContainsKey(id))
                {
                    var restApiItem = new RestApiItem(_componentManager.RestApiManager)
                    {
                        RestApiId = id,
                        ParentId = parent,
                        RestApiClass = resourceType,
                        ModuleId = moduleId,
                        Methods = methods.Distinct(),
                        Version = version,
                        Cache = cache,
                        Conditions = conditions,
                        ContextPath = new UriResource(contextPath),
                        IncludeSubPaths = includeSubPaths,
                        PathSegment = segment.ToPathSegment(),
                        Optional = optional,
                        Log = _httpServerContext?.Log
                    };

                    restApiItem.AddRestApi += (s, e) =>
                    {
                        OnAddRestApi(e);
                    };

                    restApiItem.RemoveRestApi += (s, e) =>
                    {
                        OnRemoveRestApi(e);
                    };

                    dict.Add(id, restApiItem);

                    _httpServerContext?.Log.Debug
                    (
                        I18N.Translate
                        (
                            "webexpress:restapimanager.addresource",
                            id,
                            moduleId
                        )
                    );
                }

                // assign the rest api resource to existing modules.
                foreach (var moduleContext in _componentManager.ModuleManager.GetModules(pluginContext, moduleId))
                {
                    AssignToModule(moduleContext);
                }
            }
        }

        /// <summary>
        /// Discovers and registers rest api resource from the specified plugin.
        /// </summary>
        /// <param name="pluginContexts">A list with plugin contexts that contain the resources.</param>
        public void Register(IEnumerable<IPluginContext> pluginContexts)
        {
            foreach (var pluginContext in pluginContexts)
            {
                Register(pluginContext);
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

            foreach (var resourceItem in _dictionary[pluginContext].Values)
            {
                resourceItem.Dispose();
            }

            _dictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Assign existing rest api resource to the module.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        private void AssignToModule(IModuleContext moduleContext)
        {
            foreach (var resourceItem in _dictionary.Values
                .SelectMany(x => x.Values)
                .Where(x => x.ModuleId.Equals(moduleContext?.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => !x.IsAssociatedWithModule(moduleContext)))
            {
                resourceItem.AddModule(moduleContext);
            }
        }

        /// <summary>
        /// Remove an existing modules to the application.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        private void DetachFromModule(IModuleContext moduleContext)
        {
            foreach (var resourceItem in _dictionary.Values
                .SelectMany(x => x.Values)
                .Where(x => !x.IsAssociatedWithModule(moduleContext)))
            {
                resourceItem.DetachModule(moduleContext);
            }
        }

        /// <summary>
        /// Returns an enumeration of all containing rest api resource items of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose rest api resources are to be registered.</param>
        /// <returns>An enumeration of rest api resource items.</returns>
        private IEnumerable<RestApiItem> GetPageItems(IPluginContext pluginContext)
        {
            if (!_dictionary.ContainsKey(pluginContext))
            {
                return [];
            }

            return _dictionary[pluginContext].Values;
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
        /// Collects and prepares information about the component for output in the log.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The depth of the log.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
            foreach (var resourcenItem in GetPageItems(pluginContext))
            {
                output.Add
                (
                    string.Empty.PadRight(deep) +
                    I18N.Translate
                    (
                        "webexpress:restapimanager.resource",
                        resourcenItem.RestApiId,
                        string.Join(",", resourcenItem.ModuleId)
                    )
                );
            }
        }
    }
}
