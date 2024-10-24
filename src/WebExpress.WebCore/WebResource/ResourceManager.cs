using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebResource.Model;
using WebExpress.WebCore.WebStatusPage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebResource
{
    /// <summary>
    /// The resource manager manages WebExpress elements, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public sealed class ResourceManager : IResourceManager, ISystemComponent
    {
        private readonly IComponentHub _componentHub;
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
            .SelectMany(x => x.Values)
            .Select(x => x.ResourceContext);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private ResourceManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;

            _componentHub.EndpointManager.Register<ResourceContext>
            (
                new EndpointRegistration()
                {
                    EndpointResolver = (type, applicationContext) => applicationContext != null ? GetResorces(type, applicationContext) : GetResorces(type),
                    EndpointsResolver = () => Resources,
                    HandleRequest = (request, endpointContext) =>
                    {
                        var resourceContext = endpointContext as IResourceContext;
                        var resource = CreateResourceInstance(resourceContext, request.Culture) as IResource;

                        return resource.Process(request);
                    }
                }
            );

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress:resourcemanager.initialization")
            );
        }

        /// <summary>
        /// Discovers and binds resources to an application.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin whose resources are to be associated.</param>
        private void Register(IPluginContext pluginContext)
        {
            if (_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            Register(pluginContext, _componentHub.ApplicationManager.GetApplications(pluginContext));
        }

        /// <summary>
        /// Discovers and binds resources to an application.
        /// </summary>
        /// <param name="applicationContext">The context of the application whose resources are to be associated.</param>
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
        /// Registers resources for a given plugin and application context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context (optional).</param>
        private void Register(IPluginContext pluginContext, IEnumerable<IApplicationContext> applicationContexts)
        {
            var assembly = pluginContext?.Assembly;

            foreach (var resourceType in assembly.GetTypes()
                .Where(x => x.IsClass && x.IsSealed && x.IsPublic)
                .Where(x => x.GetInterface(typeof(IResource).Name) != null)
                .Where(x => x.GetInterface(typeof(IStatusPage).Name) == null))
            {
                var id = resourceType.FullName?.ToLower();
                var segment = default(ISegmentAttribute);
                var parent = default(Type);
                var contextPath = string.Empty;
                var includeSubPaths = false;
                var conditions = new List<ICondition>();
                var cache = false;

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
                    else if (customAttribute.AttributeType == typeof(CacheAttribute))
                    {
                        cache = true;
                    }
                }

                // assign the job to existing applications
                foreach (var applicationContext in _componentHub.ApplicationManager.GetApplications(pluginContext))
                {
                    var resourceContext = new ResourceContext(_componentHub.EndpointManager, parent, new UriResource(contextPath), segment.ToPathSegment())
                    {
                        EndpointId = resourceType.FullName.ToLower(),
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext
                    };
                    var resourceItem = new ResourceItem(_componentHub.ResourceManager)
                    {
                        ParentType = parent,
                        ResourceClass = resourceType,
                        ResourceContext = resourceContext,
                        Cache = cache,
                        Conditions = conditions,
                        ContextPath = new UriResource(contextPath),
                        IncludeSubPaths = includeSubPaths,
                        PathSegment = segment.ToPathSegment(),
                        Log = _httpServerContext?.Log
                    };

                    if (_dictionary.AddResourceItem(pluginContext, applicationContext, resourceItem))
                    {
                        OnAddResource(resourceContext);
                        _httpServerContext?.Log.Debug(
                            I18N.Translate(
                                "webexpress:resourcemanager.addresource",
                                id,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Removes all resources associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the resources to remove.</param>
        internal void Remove(IPluginContext pluginContext)
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
                OnRemoveResource(resourceItem.ResourceContext);
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
                        OnRemoveResource(resourceItem.ResourceContext);
                        resourceItem.Dispose();
                    }
                }

                pluginDict.Remove(applicationContext);
            }
        }

        /// <summary>
        /// Returns an enumeration of all containing resource items of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose resources are to be registered.</param>
        /// <returns>An enumeration of resource items.</returns>
        private IEnumerable<ResourceItem> GetResorceItems(IPluginContext pluginContext)
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
        /// Returns an enumeration of all containing resource contexts of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose resources are to be registered.</param>
        /// <returns>An enumeration of resource contexts.</returns>
        public IEnumerable<IResourceContext> GetResorces(IPluginContext pluginContext)
        {
            if (_dictionary.TryGetValue(pluginContext, out var pluginResources))
            {
                return pluginResources
                    .SelectMany(x => x.Value)
                    .Select(x => x.Value.ResourceContext);
            }

            return [];
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
                .SelectMany(x => x.Values)
                .Where(x => x.ResourceClass.Equals(resourceType))
                .Select(x => x.ResourceContext);
        }

        /// <summary>
        /// Returns an enumeration of resource contextes.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of resource contextes.</returns>
        public IEnumerable<IResourceContext> GetResorces(Type resourceType, IApplicationContext applicationContext)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.ResourceClass.Equals(resourceType))
                .Where(x => x.ResourceContext.ApplicationContext.Equals(applicationContext))
                .Select(x => x.ResourceContext);
        }

        /// <summary>
        /// Returns an enumeration of resource contextes.
        /// </summary>
        /// <typeparam name="T">The resource type.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of resource contextes.</returns>
        public IEnumerable<IResourceContext> GetResorces<T>(IApplicationContext applicationContext) where T : IResource
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.ResourceClass.Equals(typeof(T)))
                .Where(x => x.ResourceContext.ApplicationContext.Equals(applicationContext))
                .Select(x => x.ResourceContext);
        }


        /// <summary>
        /// Returns the resource context.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <returns>An resource context or null.</returns>
        public IResourceContext GetResorce(IApplicationContext applicationContext, string resourceId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.ResourceContext.ApplicationContext.Equals(applicationContext))
                .Where(x => x.ResourceContext.EndpointId.Equals(resourceId))
                .Select(x => x.ResourceContext)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns the resource context.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <returns>An resource context or null.</returns>
        public IResourceContext GetResorce(string applicationId, string resourceId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.ResourceContext.ApplicationContext.ApplicationId.Equals(applicationId))
                .Where(x => x.ResourceContext.EndpointId.Equals(resourceId))
                .Select(x => x.ResourceContext)
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
                .SelectMany(x => x.Values)
                .FirstOrDefault(x => x.ResourceContext.Equals(resourceContext));

            if (resourceItem != null && resourceItem.Instance == null)
            {
                var instance = ComponentActivator.CreateInstance<IResource, IResourceContext>(resourceItem.ResourceClass, resourceContext, _componentHub);

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
                        resourcenItem?.ResourceContext?.EndpointId,
                        string.Join(",", resourcenItem.ResourceContext?.ApplicationContext?.ApplicationId)
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
