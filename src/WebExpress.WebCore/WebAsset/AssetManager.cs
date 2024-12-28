using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAsset.Model;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebResource;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebAsset
{
    /// <summary>
    /// The asset manager manages WebExpress elements, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public sealed class AssetManager : IAssetManager, ISystemComponent
    {
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly AssetItemDictionary _itemDictionary = [];
        private readonly AssetEndpointDictionary _endpointDictionary = [];

        /// <summary>
        /// An event that fires when an asset is added.
        /// </summary>
        public event EventHandler<IAssetContext> AddResource;

        /// <summary>
        /// An event that fires when an asset is removed.
        /// </summary>
        public event EventHandler<IAssetContext> RemoveResource;

        /// <summary>
        /// Returns all asset contexts.
        /// </summary>
        public IEnumerable<IAssetContext> Assets => _itemDictionary.Values
            .SelectMany(x => x.Values)
            .SelectMany(x => x)
            .Select(x => x.AssetContext);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private AssetManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;

            var endpointtRegistration = new EndpointRegistration()
            {
                EndpointResolver = (type, applicationContext) => _endpointDictionary
                    .Where(x => x.Key == applicationContext)
                    .Select(x => x.Value)
                    .Where(x => x.Item2.GetType() == type)
                    .Select(x => x.Item1),
                EndpointsResolver = () => _endpointDictionary
                    .Select(x => x.Value)
                    .Select(x => x.Item1),
                HandleRequest = (request, endpointContext) =>
                {
                    var assetContext = endpointContext as IAssetContext;
                    var asset = _itemDictionary.Values
                        .SelectMany(x => x.Values)
                        .SelectMany(x => x)
                        .FirstOrDefault(x => request.Uri.ToString().ToLower().Replace('/', '.').EndsWith(x.AssetContext.EndpointId.ToString()));

                    if (asset != null)
                    {
                        return asset.Instance.Process(request);
                    }

                    return new ResponseNotFound();
                }
            };

            AddResource += (sender, e) => endpointtRegistration.AddEndpoint?.Invoke(sender, e);
            RemoveResource += (sender, e) => endpointtRegistration.RemoveEndpoint?.Invoke(sender, e);

            _componentHub.EndpointManager.Register<AssetContext>(endpointtRegistration);

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress.webcore:assetmanager.initialization")
            );
        }

        /// <summary>
        /// Discovers and binds resources to an application.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin whose resources are to be associated.</param>
        private void Register(IPluginContext pluginContext)
        {
            if (_itemDictionary.ContainsKey(pluginContext))
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
                if (_itemDictionary.TryGetValue(pluginContext, out var appDict) && appDict.ContainsKey(applicationContext))
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
        /// <param name="applicationContexts">The application context (optional).</param>
        private void Register(IPluginContext pluginContext, IEnumerable<IApplicationContext> applicationContexts)
        {
            var assembly = pluginContext?.Assembly;
            var assemblName = assembly.GetName().Name;
            var embeddedResources = assembly.GetManifestResourceNames();

            foreach (var resource in embeddedResources)
            {
                if (resource.StartsWith(assemblName + ".Assets.", StringComparison.OrdinalIgnoreCase))
                {
                    var id = resource[(assemblName.Length + 8)..];

                    // assign the asset to existing applications
                    foreach (var applicationContext in applicationContexts)
                    {
                        var assetContext = new AssetContext(new UriResource(), new UriPathSegmentConstant($"assets/{id}"))
                        {
                            EndpointId = new ComponentId(id),
                            PluginContext = pluginContext,
                            ApplicationContext = applicationContext,
                            IncludeSubPaths = false
                        };

                        var assetItem = new AssetItem(_componentHub.AssetManager)
                        {
                            AssetClass = typeof(Asset),
                            AssetContext = assetContext,
                            Instance = ComponentActivator.CreateInstance<IAsset, IAssetContext>(typeof(Asset), assetContext, _httpServerContext, _componentHub, resource)
                        };

                        if (_itemDictionary.AddAssetItem(pluginContext, applicationContext, assetItem))
                        {
                            OnAddResource(assetContext);
                            _httpServerContext?.Log.Debug(
                                I18N.Translate(
                                    "webexpress.webcore:assetmanager.addresource",
                                    id,
                                    applicationContext.ApplicationId
                                )
                            );
                        }
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
            if (_itemDictionary.TryGetValue(pluginContext, out var value))
            {
                foreach (var resourceItem in value.Values
                    .SelectMany(x => x))
                {
                    OnRemoveResource(resourceItem.AssetContext);
                    resourceItem.Dispose();
                }

                _itemDictionary.Remove(pluginContext);
            }
        }

        /// <summary>
        /// Removes all assets associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the resources to remove.</param>
        internal void Remove(IApplicationContext applicationContext)
        {
            if (applicationContext == null)
            {
                return;
            }

            foreach (var pluginDict in _itemDictionary.Values)
            {
                foreach (var assetList in pluginDict.Where(x => x.Key == applicationContext).Select(x => x.Value))
                {
                    foreach (var assetItem in assetList)
                    {
                        OnRemoveResource(assetItem.AssetContext);
                        assetItem.Dispose();
                    }
                }

                pluginDict.Remove(applicationContext);
            }
        }

        /// <summary>
        /// Returns an enumeration of all containing asset items of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose resources are to be registered.</param>
        /// <returns>An enumeration of resource items.</returns>
        private IEnumerable<AssetItem> GetAssetItems(IPluginContext pluginContext)
        {
            if (_itemDictionary.TryGetValue(pluginContext, out var pluginResources))
            {
                return pluginResources
                    .SelectMany(x => x.Value)
                    .Select(x => x);
            }

            return [];
        }

        /// <summary>
        /// Returns an enumeration of all containing asset contexts of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose asset are to be registered.</param>
        /// <returns>An enumeration of asset contexts.</returns>
        public IEnumerable<IAssetContext> GetAssets(IPluginContext pluginContext)
        {
            if (_itemDictionary.TryGetValue(pluginContext, out var pluginResources))
            {
                return pluginResources
                    .SelectMany(x => x.Value)
                    .Select(x => x.AssetContext);
            }

            return [];
        }

        /// <summary>
        /// Returns an enumeration of asset contextes.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of asset contextes.</returns>
        public IEnumerable<IAssetContext> GetAssets(IApplicationContext applicationContext)
        {
            return _itemDictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x)
                .Where(x => x.AssetContext.ApplicationContext.Equals(applicationContext))
                .Select(x => x.AssetContext);
        }

        /// <summary>
        /// Creates a new resource and returns it. If a resource already exists (through caching), the existing instance is returned.
        /// </summary>
        /// <param name="assetContext">The context used for asset creation.</param>
        /// <returns>The created or cached resource.</returns>
        private IAsset CreateAssetInstance(IAssetContext assetContext)
        {
            var resourceItem = _itemDictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x)
                .FirstOrDefault(x => x.AssetContext.Equals(assetContext));

            if (resourceItem != null && resourceItem.Instance == null)
            {
                var instance = ComponentActivator.CreateInstance<IAsset, IAssetContext>(resourceItem.AssetClass, assetContext, _httpServerContext, _componentHub);
                resourceItem.Instance = instance;

                return instance;
            }

            return resourceItem?.Instance as IAsset;
        }

        /// <summary>
        /// Raises the AddResource event.
        /// </summary>
        /// <param name="resourceContext">The asset context.</param>
        private void OnAddResource(IAssetContext resourceContext)
        {
            AddResource?.Invoke(this, resourceContext);
        }

        /// <summary>
        /// Raises the RemoveResource event.
        /// </summary>
        /// <param name="resourceContext">The asset context.</param>
        private void OnRemoveResource(IAssetContext resourceContext)
        {
            RemoveResource?.Invoke(this, resourceContext);
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
        /// Raises the event when an application is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being added.</param>
        private void OnAddApplication(object sender, IApplicationContext e)
        {
            Register(e);

            var assembly = typeof(AssetManager).Assembly;
            var assemblyName = assembly.GetName().Name.ToLower();

            var context = new AssetContext(new UriResource(), new UriPathSegmentConstant("assets"))
            {
                ApplicationContext = e,
                PluginContext = new PluginContext()
                {
                    PluginId = new ComponentId(assemblyName),
                    Assembly = assembly
                },
                EndpointId = new ComponentId(assemblyName + ".asset"),
                IncludeSubPaths = true
            };

            var asset = ComponentActivator.CreateInstance<IAsset, IAssetContext>(typeof(Asset), context, _httpServerContext, _componentHub);

            _endpointDictionary.TryAdd(e, (context, asset));
        }

        /// <summary>
        /// Raises the event when an application is removed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being removed.</param>
        private void OnRemoveApplication(object sender, IApplicationContext e)
        {
            Remove(e);

            _endpointDictionary.Remove(e);
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        private void Log()
        {
            //foreach (var resourcenItem in GetResorceItems(pluginContext))
            //{
            //    output.Add
            //    (
            //        string.Empty.PadRight(deep) +
            //        I18N.Translate
            //        (
            //            "webexpress.webcore:resourcemanager.resource",
            //            resourcenItem?.ResourceContext?.EndpointId,
            //            string.Join(",", resourcenItem.ResourceContext?.ApplicationContext?.ApplicationId)
            //        )
            //    );
            //}
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
