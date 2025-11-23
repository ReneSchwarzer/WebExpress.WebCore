using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAsset.Model;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPlugin;
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
        private readonly AssetItemDictionary _itemDictionary = new();

        /// <summary>
        /// An event that fires when an asset is added.
        /// </summary>
        public event EventHandler<IAssetContext> AddAsset;

        /// <summary>
        /// An event that fires when an asset is removed.
        /// </summary>
        public event EventHandler<IAssetContext> RemoveAsset;

        /// <summary>
        /// Returns all asset contexts.
        /// </summary>
        public IEnumerable<IAssetContext> Assets => _itemDictionary.All.Select(x => x.AssetContext);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used via Reflection.")]
        private AssetManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;

            var endpointtRegistration = new EndpointRegistration()
            {
                EndpointResolver = (type, applicationContext) => [],
                EndpointsResolver = () => Assets,
                HandleRequest = (request, endpointContext) =>
                {
                    var assetContext = endpointContext as IAssetContext;
                    var asset = _itemDictionary.All
                        .FirstOrDefault
                        (
                            x =>
                            request.Uri
                                .ToString()
                                .ToLower()
                                .Replace("/", ".")
                                .EndsWith(x.AssetContext.Route.ToString().Replace("/", "."))
                        );

                    if (asset is not null)
                    {
                        return asset.Instance.Process(request);
                    }

                    return new ResponseNotFound();
                }
            };

            AddAsset += (sender, e) => endpointtRegistration.AddEndpoint?.Invoke(sender, e);
            RemoveAsset += (sender, e) => endpointtRegistration.RemoveEndpoint?.Invoke(sender, e);

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
            if (_itemDictionary.ContainsPlugin(pluginContext))
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
                if (_itemDictionary.ContainsApplication(pluginContext, applicationContext))
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
                    var id = resource[(assemblName.Length + 8)..]?.Replace('\\', '/');

                    // assign the asset to existing applications
                    foreach (var applicationContext in applicationContexts)
                    {
                        var pluginPath = applicationContext.PluginContext != pluginContext
                            ? pluginContext.PluginId.ToString()
                            : null;

                        var prefix = applicationContext.Route
                            .Concat(pluginPath)
                            .Concat(new UriPathSegmentConstant("assets"));

                        var assetContext = new AssetContext()
                        {
                            EndpointId = new ComponentId($"{pluginContext.PluginId}.{id.Replace('/', '.')}"),
                            PluginContext = pluginContext,
                            ApplicationContext = applicationContext,
                            Route = prefix.Concat(id),
                            IncludeSubPaths = false
                        };

                        var assetItem = new AssetItem(_componentHub.AssetManager)
                        {
                            AssetClass = typeof(Asset),
                            AssetContext = assetContext,
                            Instance = ComponentActivator.CreateInstance<IAsset, IAssetContext>
                            (
                                typeof(Asset),
                                assetContext,
                                _httpServerContext,
                                _componentHub,
                                resource
                            )
                        };

                        if (_itemDictionary.AddAssetItem(pluginContext, applicationContext, assetItem))
                        {
                            OnAddAsset(assetContext);
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
            foreach (var assetContext in _itemDictionary.Remove(pluginContext))
            {
                OnRemoveAsset(assetContext);
            }
        }

        /// <summary>
        /// Removes all assets associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the resources to remove.</param>
        internal void Remove(IApplicationContext applicationContext)
        {
            foreach (var assetContext in _itemDictionary.Remove(applicationContext))
            {
                OnRemoveAsset(assetContext);
            }
        }

        /// <summary>
        /// Returns an enumeration of all containing asset contexts of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose asset are to be registered.</param>
        /// <returns>An enumeration of asset contexts.</returns>
        public IEnumerable<IAssetContext> GetAssets(IPluginContext pluginContext)
        {
            return _itemDictionary.GetAssets(pluginContext);
        }

        /// <summary>
        /// Returns an enumeration of asset contextes.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of asset contextes.</returns>
        public IEnumerable<IAssetContext> GetAssets(IApplicationContext applicationContext)
        {
            return _itemDictionary.GetAssets(applicationContext);
        }

        /// <summary>
        /// Raises the AddAsset event.
        /// </summary>
        /// <param name="assetContext">The asset context.</param>
        private void OnAddAsset(IAssetContext assetContext)
        {
            AddAsset?.Invoke(this, assetContext);
        }

        /// <summary>
        /// Raises the RemoveAsset event.
        /// </summary>
        /// <param name="assetContext">The asset context.</param>
        private void OnRemoveAsset(IAssetContext assetContext)
        {
            RemoveAsset?.Invoke(this, assetContext);
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
