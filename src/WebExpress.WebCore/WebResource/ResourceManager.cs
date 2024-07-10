using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebScope;
using WebExpress.WebCore.WebStatusPage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebResource
{
    /// <summary>
    /// The resource manager manages WebExpress elements, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public sealed class ResourceManager : IComponentPlugin, ISystemComponent
    {
        /// <summary>
        /// An event that fires when an resource is added.
        /// </summary>
        public event EventHandler<IResourceContext> AddResource;

        /// <summary>
        /// An event that fires when an resource is removed.
        /// </summary>
        public event EventHandler<IResourceContext> RemoveResource;

        /// <summary>
        /// Returns the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns the directory where the resources are listed.
        /// </summary>
        private ResourceDictionary Dictionary { get; } = new ResourceDictionary();

        /// <summary>
        /// Returns all resource items.
        /// </summary>
        internal IEnumerable<ResourceItem> ResourceItems => Dictionary.Values.SelectMany(x => x.Values);

        /// <summary>
        /// Returns all resource contexts.
        /// </summary>
        public IEnumerable<IResourceContext> Resources => Dictionary.Values
            .SelectMany(x => x.Values)
            .SelectMany(x => x.ResourceContexts);

        /// <summary>
        /// Constructor
        /// </summary>
        internal ResourceManager()
        {
            ComponentManager.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            ComponentManager.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };

            ComponentManager.ModuleManager.AddModule += (sender, moduleContext) =>
            {
                AssignToModule(moduleContext);
            };

            ComponentManager.ModuleManager.RemoveModule += (sender, moduleContext) =>
            {
                DetachFromModule(moduleContext);
            };
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The reference to the context of the host.</param>
        public void Initialization(IHttpServerContext context)
        {
            HttpServerContext = context;

            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N("webexpress:resourcemanager.initialization")
            );
        }

        /// <summary>
        /// Discovers and registers resources from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose resources are to be registered.</param>
        public void Register(IPluginContext pluginContext)
        {
            if (Dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            var assembly = pluginContext?.Assembly;

            Dictionary.Add(pluginContext, new Dictionary<string, ResourceItem>());
            var dict = Dictionary[pluginContext];

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
                    HttpServerContext.Log.Warning
                    (
                        InternationalizationManager.I18N
                        (
                            "webexpress:resourcemanager.moduleless",
                            id
                        )
                    );

                    continue;
                }

                if (!dict.ContainsKey(id))
                {
                    var resourceItem = new ResourceItem()
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
                        Log = HttpServerContext.Log
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

                    HttpServerContext.Log.Debug
                    (
                        InternationalizationManager.I18N
                        (
                            "webexpress:resourcemanager.addresource",
                            id,
                            moduleId
                        )
                    );
                }

                // assign the resource to existing modules.
                foreach (var moduleContext in ComponentManager.ModuleManager.GetModules(pluginContext, moduleId))
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
        /// Assign existing resources to the module.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        private void AssignToModule(IModuleContext moduleContext)
        {
            foreach (var resourceItem in Dictionary.Values
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
            foreach (var resourceItem in Dictionary.Values
                .SelectMany(x => x.Values)
                .Where(x => !x.IsAssociatedWithModule(moduleContext)))
            {
                resourceItem.DetachModule(moduleContext);
            }
        }

        /// <summary>
        /// Renturns an enumeration of all containing resource items of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose resources are to be registered.</param>
        /// <returns>An enumeration of resource items.</returns>
        internal IEnumerable<ResourceItem> GetResorceItems(IPluginContext pluginContext)
        {
            if (!Dictionary.ContainsKey(pluginContext))
            {
                return new List<ResourceItem>();
            }

            return Dictionary[pluginContext].Values;
        }

        /// <summary>
        /// Renturns an enumeration of all containing resource contexts of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose resources are to be registered.</param>
        /// <returns>An enumeration of resource contexts.</returns>
        public IEnumerable<IResourceContext> GetResorces(IPluginContext pluginContext)
        {
            if (!Dictionary.ContainsKey(pluginContext))
            {
                return new List<IResourceContext>();
            }

            return Dictionary[pluginContext].Values
                .SelectMany(x => x.ResourceContexts);
        }

        /// <summary>
        /// Renturns the resource context.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <returns>An resource context or null.</returns>
        public IResourceContext GetResorces(string applicationId, string moduleId, string resourceId)
        {
            return Dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.ResourceContexts)
                .Where(x => x.ModuleContext != null && x.ModuleContext.ApplicationContext != null)
                .Where(x => x.ModuleContext.ApplicationContext.ApplicationId.Equals(applicationId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.ModuleContext.ModuleId.Equals(moduleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.ResourceId.Equals(resourceId, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

        /// <summary>
        /// Removes all resources associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the resources to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            // the plugin has not been registered in the manager
            if (!Dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            foreach (var resourceItem in Dictionary[pluginContext].Values)
            {
                resourceItem.Dispose();
            }

            Dictionary.Remove(pluginContext);
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
                    InternationalizationManager.I18N
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
