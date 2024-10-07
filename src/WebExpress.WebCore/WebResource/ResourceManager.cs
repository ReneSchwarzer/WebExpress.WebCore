using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebResource.Model;
using WebExpress.WebCore.WebScope;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebCore.WebStatusPage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebResource
{
    /// <summary>
    /// The resource manager manages WebExpress elements, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public sealed class ResourceManager : IResourceManager, IComponentManagerPlugin, ISystemComponent
    {
        private readonly IComponentHub _componentManager;
        private readonly IHttpServerContext _httpServerContext;
        private readonly ResourceDictionary _dictionary = [];

        /// <summary>
        /// An event that fires when an resource is added.
        /// </summary>
        public event EventHandler<IResourceContext> AddResource;

        /// <summary>
        /// An event that fires when an resource is removed.
        /// </summary>
        public event EventHandler<IResourceContext> RemoveResource;

        /// <summary>
        /// Returns all resource contexts.
        /// </summary>
        public IEnumerable<IResourceContext> Resources => _dictionary.Values
            .SelectMany(x => x.Values)
            .SelectMany(x => x.ResourceContexts);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentManager">The component manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private ResourceManager(IComponentHub componentManager, IHttpServerContext httpServerContext)
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

            _componentManager.SitemapManager.Register<ResourceContext>
            (
                new EndpointRegistration()
                {
                    Factory = (resourceContext, uri, culture) => CreateResourceInstance(resourceContext as IResourceContext, culture),
                    ContextResolver = (type, moduleContext) => moduleContext != null ? GetResorces(type, moduleContext) : GetResorces(type),
                    EndpointResolver = () => Resources,
                    HandleRequest = (endpoint, endpointContext, request) => { return (endpoint as IResource).Process(request); }
                }
            );

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress:resourcemanager.initialization")
            );
        }

        /// <summary>
        /// Discovers and registers resources from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose resources are to be registered.</param>
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
                .Where(x => x.GetInterface(typeof(IResource).Name) != null)
                .Where(x => x.GetInterface(typeof(IStatusPage).Name) == null))
            {
                var id = resourceType.FullName?.ToLower();
                var segment = default(ISegmentAttribute);
                var title = resourceType.Name;
                var parent = default(string);
                var contextPath = string.Empty;
                var includeSubPaths = false;
                var moduleId = string.Empty;
                var scopes = new List<string>();
                var conditions = new List<ICondition>();
                var optional = false;
                var cache = false;

                foreach (var customAttribute in resourceType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IResourceAttribute))))
                {
                    var buf = typeof(ModuleAttribute<>);

                    if (customAttribute.AttributeType.GetInterfaces().Contains(typeof(ISegmentAttribute)))
                    {
                        segment = resourceType.GetCustomAttributes(customAttribute.AttributeType, false).FirstOrDefault() as ISegmentAttribute;
                    }
                    else if (customAttribute.AttributeType == typeof(TitleAttribute))
                    {
                        title = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
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
                    else if (customAttribute.AttributeType.Name == typeof(ScopeAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ScopeAttribute<>).Namespace)
                    {
                        scopes.Add(customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower());
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
                    else if (customAttribute.AttributeType == typeof(OptionalAttribute))
                    {
                        optional = true;
                    }
                }

                if (resourceType.GetInterfaces().Where(x => x == typeof(IScope)).Any())
                {
                    scopes.Add(resourceType.FullName?.ToLower());
                }

                if (string.IsNullOrEmpty(moduleId))
                {
                    // no module specified
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate
                        (
                            "webexpress:resourcemanager.moduleless",
                            id
                        )
                    );

                    continue;
                }

                if (!dict.ContainsKey(id))
                {
                    var resourceItem = new ResourceItem(_componentManager.ResourceManager)
                    {
                        ResourceId = id,
                        Title = title,
                        ParentId = parent,
                        ResourceClass = resourceType,
                        ModuleId = moduleId,
                        Scopes = scopes,
                        Cache = cache,
                        Conditions = conditions,
                        ContextPath = new UriResource(contextPath),
                        IncludeSubPaths = includeSubPaths,
                        PathSegment = segment.ToPathSegment(),
                        Optional = optional,
                        Log = _httpServerContext?.Log
                    };

                    resourceItem.AddResource += (s, e) =>
                    {
                        OnAddResource(e);
                    };

                    resourceItem.RemoveResource += (s, e) =>
                    {
                        OnRemoveResource(e);
                    };

                    dict.Add(id, resourceItem);

                    _httpServerContext?.Log.Debug
                    (
                        I18N.Translate
                        (
                            "webexpress:resourcemanager.addresource",
                            id,
                            moduleId
                        )
                    );
                }

                // assign the resource to existing modules.
                foreach (var moduleContext in _componentManager.ModuleManager.GetModules(pluginContext, moduleId))
                {
                    AssignToModule(moduleContext);
                }
            }
        }

        /// <summary>
        /// Discovers and registers resources from the specified plugin.
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
        /// Removes all resources associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the resources to remove.</param>
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
        /// Assign existing resources to the module.
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
        /// Returns an enumeration of all containing resource items of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose resources are to be registered.</param>
        /// <returns>An enumeration of resource items.</returns>
        private IEnumerable<ResourceItem> GetResorceItems(IPluginContext pluginContext)
        {
            if (!_dictionary.ContainsKey(pluginContext))
            {
                return [];
            }

            return _dictionary[pluginContext].Values;
        }

        /// <summary>
        /// Returns an enumeration of all containing resource contexts of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose resources are to be registered.</param>
        /// <returns>An enumeration of resource contexts.</returns>
        public IEnumerable<IResourceContext> GetResorces(IPluginContext pluginContext)
        {
            if (!_dictionary.ContainsKey(pluginContext))
            {
                return [];
            }

            return _dictionary[pluginContext].Values
                .SelectMany(x => x.ResourceContexts);
        }

        /// <summary>
        /// Returns an enumeration of resource contextes.
        /// </summary>
        /// <typeparam name="T">The resource type.</typeparam>
        /// <returns>An enumeration of resource contextes.</returns>
        public IEnumerable<IResourceContext> GetResorces<T>() where T : IResource
        {
            return GetResorces(typeof(T));
        }

        /// <summary>
        /// Returns an enumeration of resource contextes.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <returns>An enumeration of resource contextes.</returns>
        public IEnumerable<IResourceContext> GetResorces(Type resourceType)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.ResourceContexts, (x, y) => new { x.ResourceClass, ResourceContext = y })
                .Where(x => x.ResourceClass.Equals(resourceType))
                .Select(x => x.ResourceContext);
        }

        /// <summary>
        /// Returns an enumeration of resource contextes.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="moduleContext">The context of the module.</param>
        /// <returns>An enumeration of resource contextes.</returns>
        public IEnumerable<IResourceContext> GetResorces(Type resourceType, IModuleContext moduleContext)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.ResourceContexts, (x, y) => new { x.ResourceClass, ResourceContext = y })
                .Where(x => x.ResourceContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.ResourceClass.Equals(resourceType))
                .Select(x => x.ResourceContext);
        }

        /// <summary>
        /// Returns an enumeration of resource contextes.
        /// </summary>
        /// <typeparam name="T">The resource type.</typeparam>
        /// <param name="moduleContext">The context of the module.</param>
        /// <returns>An enumeration of resource contextes.</returns>
        public IEnumerable<IResourceContext> GetResorces<T>(IModuleContext moduleContext) where T : IResource
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.ResourceContexts, (x, y) => new { x.ResourceClass, ResourceContext = y })
                .Where(x => x.ResourceContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.ResourceClass.Equals(typeof(T)))
                .Select(x => x.ResourceContext);
        }

        /// <summary>
        /// Returns the resource context.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <returns>An resource context or null.</returns>
        public IResourceContext GetResorce(IModuleContext moduleContext, Type resourceType)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.ResourceContexts, (x, y) => new { x.ResourceClass, ResourceContext = y })
                .Where(x => x.ResourceContext.ModuleContext?.ApplicationContext != null)
                .Where(x => x.ResourceContext.ModuleContext.ApplicationContext.ApplicationId.Equals(moduleContext.ApplicationContext.ApplicationId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.ResourceContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.ResourceClass.Equals(resourceType))
                .Select(x => x.ResourceContext)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns the resource context.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <returns>An resource context or null.</returns>
        public IResourceContext GetResorce(IModuleContext moduleContext, string resourceId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.ResourceContexts, (x, y) => new { x.ResourceClass, ResourceContext = y })
                .Where(x => x.ResourceContext.ModuleContext?.ApplicationContext != null)
                .Where(x => x.ResourceContext.ModuleContext.ApplicationContext.ApplicationId.Equals(moduleContext.ApplicationContext.ApplicationId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.ResourceContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.ResourceContext.EndpointId.Equals(resourceId, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.ResourceContext)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns the resource context.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <returns>An resource context or null.</returns>
        public IResourceContext GetResorce(string applicationId, string moduleId, string resourceId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.ResourceContexts)
                .Where(x => x.ModuleContext != null && x.ModuleContext.ApplicationContext != null)
                .Where(x => x.ModuleContext.ApplicationContext.ApplicationId.Equals(applicationId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.ModuleContext.ModuleId.Equals(moduleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.EndpointId.Equals(resourceId, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

        /// <summary>
        /// Creates a new resource and returns it. If a resource already exists (through caching), the existing instance is returned.
        /// </summary>
        /// <param name="resourceContext">The context used for resource creation.</param>
        /// <param name="culture">The culture with the language settings.</param>
        /// <returns>The created or cached resource.</returns>
        private IResource CreateResourceInstance(IResourceContext resourceContext, CultureInfo culture)
        {
            var resourceItem = _dictionary.Values
                .SelectMany(x => x.Values)
                .FirstOrDefault(x => x.ResourceContexts.Contains(resourceContext));

            if (resourceItem != null && resourceItem.Instance == null)
            {
                var instance = ComponentActivator.CreateInstance<IResource, IResourceContext>(resourceItem.ResourceClass, resourceContext, _componentManager);

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

            return resourceItem?.Instance as IResource;
        }

        /// <summary>
        /// Raises the AddResource event.
        /// </summary>
        /// <param name="resourceContext">The resource context.</param>
        private void OnAddResource(IResourceContext resourceContext)
        {
            AddResource?.Invoke(this, resourceContext);
        }

        /// <summary>
        /// Raises the RemoveResource event.
        /// </summary>
        /// <param name="resourceContext">The resource context.</param>
        private void OnRemoveResource(IResourceContext resourceContext)
        {
            RemoveResource?.Invoke(this, resourceContext);
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
            foreach (var resourcenItem in GetResorceItems(pluginContext))
            {
                output.Add
                (
                    string.Empty.PadRight(deep) +
                    I18N.Translate
                    (
                        "webexpress:resourcemanager.resource",
                        resourcenItem.ResourceId,
                        string.Join(",", resourcenItem.ModuleId)
                    )
                );
            }
        }
    }
}
