using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent.Model;
using WebExpress.WebCore.WebEvent;
using WebExpress.WebCore.WebJob;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPackage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebResource;
using WebExpress.WebCore.WebRestApi;
using WebExpress.WebCore.WebSession;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebCore.WebStatusPage;
using WebExpress.WebCore.WebTask;

namespace WebExpress.WebCore.WebComponent
{
    /// <summary>
    /// Central management of components.
    /// </summary>
    public class ComponentHub : IComponentHub
    {
        private readonly ComponentDictionary _dictionary = [];
        private readonly InternationalizationManager _internationalizationManager;
        private readonly PluginManager _pluginManager;
        private readonly ApplicationManager _applicationManager;
        private readonly ModuleManager _moduleManager;
        private readonly ResourceManager _resourceManager;
        private readonly PageManager _pageManager;
        private readonly RestApiManager _restApiManager;
        private readonly SitemapManager _sitemapManager;
        private readonly StatusPageManager _statusPageManager;

        /// <summary>
        /// An event that fires when an component is added.
        /// </summary>
        public event EventHandler<IComponentManager> AddComponent;

        /// <summary>
        /// An event that fires when an component is removed.
        /// </summary>
        public event EventHandler<IComponentManager> RemoveComponent;

        /// <summary>
        /// Returns the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns all registered managers.
        /// </summary>
        public IEnumerable<IComponentManager> Managers => new IComponentManager[]
            {
                LogManager,
                PackageManager,
                _pluginManager,
                _applicationManager,
                _moduleManager,
                _sitemapManager,
                _resourceManager,
                _pageManager,
                _restApiManager,
                EventManager,
                JobManager,
                _statusPageManager,
                _internationalizationManager,
                SessionManager,
                TaskManager
            }.Concat(_dictionary.Values.SelectMany(x => x).Select(x => x.ComponentInstance));

        /// <summary>
        /// Returns the log manager.
        /// </summary>
        /// <returns>The instance of the log manager.</returns>
        public LogManager LogManager { get; private set; }

        /// <summary>
        /// Returns the package manager.
        /// </summary>
        /// <returns>The instance of the package manager.</returns>
        public PackageManager PackageManager { get; private set; }

        /// <summary>
        /// Returns the plugin manager.
        /// </summary>
        /// <returns>The instance of the plugin manager.</returns>
        public IPluginManager PluginManager => _pluginManager;

        /// <summary>
        /// Returns the application manager.
        /// </summary>
        /// <returns>The instance of the application manager.</returns>
        public IApplicationManager ApplicationManager => _applicationManager;

        /// <summary>
        /// Returns the module manager.
        /// </summary>
        /// <returns>The instance of the module manager.</returns>
        public IModuleManager ModuleManager => _moduleManager;

        /// <summary>
        /// Returns the event manager.
        /// </summary>
        /// <returns>The instance of the event manager.</returns>
        public EventManager EventManager { get; private set; }

        /// <summary>
        /// Returns the job manager.
        /// </summary>
        /// <returns>The instance of the job manager.</returns>
        public JobManager JobManager { get; private set; }

        /// <summary>
        /// Returns the resource manager.
        /// </summary>
        /// <returns>The instance of the resource manager.</returns>
        public IResourceManager ResourceManager => _resourceManager;

        /// <summary>
        /// Returns the page manager.
        /// </summary>
        /// <returns>The instance of the page manager.</returns>
        public IPageManager PageManager => _pageManager;

        /// <summary>
        /// Returns the rest api manager.
        /// </summary>
        /// <returns>The instance of the rest api manager.</returns>
        public IRestApiManager RestApiManager => _restApiManager;

        /// <summary>
        /// Returns the sitemap manager.
        /// </summary>
        /// <returns>The instance of the sitemap manager.</returns>
        public ISitemapManager SitemapManager => _sitemapManager;

        /// <summary>
        /// Returns the status page manager.
        /// </summary>
        /// <returns>The instance of the status page manager.</returns>
        public IStatusPageManager StatusPageManager => _statusPageManager;

        /// <summary>
        /// Returns the internationalization manager.
        /// </summary>
        /// <returns>The instance of the internationalization manager.</returns>
        public IInternationalizationManager InternationalizationManager => _internationalizationManager;

        /// <summary>
        /// Returns the session manager.
        /// </summary>
        /// <returns>The instance of the session manager.</returns>
        public SessionManager SessionManager { get; private set; }

        /// <summary>
        /// Returns the task manager.
        /// </summary>
        /// <returns>The instance of the task manager manager.</returns>
        public TaskManager TaskManager { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        internal ComponentHub(IHttpServerContext httpServerContext)
        {
            HttpServerContext = httpServerContext;

            // order is relevant
            LogManager = CreateInstance(typeof(LogManager)) as LogManager;
            PackageManager = CreateInstance(typeof(PackageManager)) as PackageManager;
            _pluginManager = CreateInstance(typeof(PluginManager)) as PluginManager;
            _internationalizationManager = CreateInstance(typeof(InternationalizationManager)) as InternationalizationManager;
            _applicationManager = CreateInstance(typeof(ApplicationManager)) as ApplicationManager;
            _moduleManager = CreateInstance(typeof(ModuleManager)) as ModuleManager;
            _sitemapManager = CreateInstance(typeof(SitemapManager)) as SitemapManager;
            _resourceManager = CreateInstance(typeof(ResourceManager)) as ResourceManager;
            _pageManager = CreateInstance(typeof(PageManager)) as PageManager;
            _restApiManager = CreateInstance(typeof(RestApiManager)) as RestApiManager;
            _statusPageManager = CreateInstance(typeof(StatusPageManager)) as StatusPageManager;
            EventManager = CreateInstance(typeof(EventManager)) as EventManager;
            JobManager = CreateInstance(typeof(JobManager)) as JobManager;
            SessionManager = CreateInstance(typeof(SessionManager)) as SessionManager;
            TaskManager = CreateInstance(typeof(TaskManager)) as TaskManager;

            _internationalizationManager.Register(typeof(HttpServer).Assembly, "webexpress");

            HttpServerContext.Log.Debug
            (
                _internationalizationManager.Translate("webexpress:componentmanager.initialization")
            );

            _pluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };
        }

        /// <summary>
        /// Creates and initializes a component.
        /// </summary>
        /// <param name="componentType">The component class.</param>
        /// <returns>The instance of the create and initialized component.</returns>
        private IComponentManager CreateInstance(Type componentType)
        {
            if (componentType == null)
            {
                return null;
            }
            else if (!componentType.GetInterfaces().Where(x => x == typeof(IComponentManager)).Any())
            {
                HttpServerContext.Log.Warning
                (
                    _internationalizationManager.Translate
                    (
                        "webexpress:componentmanager.wrongtype",
                        componentType?.FullName, typeof(IComponentManager).FullName
                    )
                );

                return null;
            }

            try
            {
                return ComponentActivator.CreateInstance<IComponentManager>(componentType, this);
            }
            catch (Exception ex)
            {
                HttpServerContext.Log.Exception(ex);
            }

            return null;
        }

        /// <summary>
        /// Returns a component based on its id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The instance of the component or null.</returns>
        public IComponentManager GetComponent(string id)
        {
            return _dictionary.Values
                .SelectMany(x => x)
                .Where(x => x.ComponentId.Equals(id, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.ComponentInstance)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns a component based on its type.
        /// </summary>
        /// <typeparam name="T">The component class.</typeparam>
        /// <returns>The instance of the component or null.</returns>
        public T GetComponent<T>() where T : IComponentManager
        {
            return (T)_dictionary.Values
                .SelectMany(x => x)
                .Where(x => x.ComponentClass == typeof(T))
                .Select(x => x.ComponentInstance)
                .FirstOrDefault();
        }

        /// <summary>
        /// Discovers and registers the components from the specified plugin.
        /// </summary>
        /// <param name="pluginContexts">A plugin context that contain the components.</param>
        internal void Register(IPluginContext pluginContext)
        {
            // the plugin has already been registered
            if (_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            var assembly = pluginContext.Assembly;

            _dictionary.Add(pluginContext, []);
            var componentItems = _dictionary[pluginContext];

            foreach (var type in assembly.GetExportedTypes().Where(x => x.IsClass && x.IsSealed && x.GetInterface(typeof(IComponentManager).Name) != null))
            {
                var id = type.FullName?.ToLower();

                // determining attributes
                var componentInstance = CreateInstance(type);

                if (!componentItems.Where(x => x.ComponentId.Equals(id, StringComparison.OrdinalIgnoreCase)).Any())
                {
                    componentItems = componentItems.Concat([ new ComponentItem()
                    {
                        ComponentClass = type,
                        ComponentId = id,
                        ComponentInstance = componentInstance
                    }]);

                    HttpServerContext.Log.Debug
                    (
                        _internationalizationManager.Translate("webexpress:componentmanager.register", id)
                    );

                    // raises the AddComponent event
                    OnAddComponent(componentInstance);
                }
                else
                {
                    HttpServerContext.Log.Warning
                    (
                        _internationalizationManager.Translate("webexpress:componentmanager.duplicate", id)
                    );
                }
            }
        }

        /// <summary>
        /// Discovers and registers the components from the specified plugins.
        /// </summary>
        /// <param name="pluginContexts">A list with plugin contexts that contain the components.</param>
        public void Register(IEnumerable<IPluginContext> pluginContexts)
        {
            foreach (var pluinContext in pluginContexts)
            {
                Register(pluinContext);
            }
        }

        /// <summary>
        /// Boots the components.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        internal void BootComponent(IPluginContext pluginContext)
        {
            _pluginManager.Boot(pluginContext);
            _applicationManager.Boot(pluginContext);
            _moduleManager.Boot(pluginContext);

            foreach (var component in _dictionary.Values
                .Where(x => x is IExecutableElements)
                .Select(x => x as IExecutableElements))
            {
                component.Boot(pluginContext);
            }
        }

        /// <summary>
        /// Boots the components.
        /// </summary>
        /// <param name="pluginContexts">A enumeration of plugin contexts.</param>
        internal void BootComponent(IEnumerable<IPluginContext> pluginContexts)
        {
            foreach (var pluginContext in pluginContexts)
            {
                BootComponent(pluginContext);
            }
        }

        /// <summary>
        /// Starts the component.
        /// </summary>
        internal void Execute()
        {
            HttpServerContext.Log.Debug
            (
                _internationalizationManager.Translate("webexpress:componentmanager.execute")
            );

            PackageManager.Execute();
            JobManager.Execute();
        }

        /// <summary>
        /// Shutting down the component manager.
        /// </summary>
        internal void ShutDown()
        {
            HttpServerContext.Log.Debug
            (
                _internationalizationManager.Translate("webexpress:componentmanager.shutdown")
            );
        }

        /// <summary>
        /// Shutting down the component.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        internal void ShutDownComponent(IPluginContext pluginContext)
        {
            _pluginManager.ShutDown(pluginContext);
            _applicationManager.ShutDown(pluginContext);
            _moduleManager.ShutDown(pluginContext);

            foreach (var component in _dictionary.Values
                .Where(x => x is IExecutableElements)
                .Select(x => x as IExecutableElements))
            {
                component.ShutDown(pluginContext);
            }
        }

        /// <summary>
        /// Removes all components associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the applications to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            if (pluginContext == null)
            {
                return;
            }

            if (_dictionary.TryGetValue(pluginContext, out IEnumerable<ComponentItem> componentItems))
            {
                if (!componentItems.Any())
                {
                    return;
                }

                foreach (var componentItem in componentItems)
                {
                    OnRemoveComponent(componentItem.ComponentInstance);

                    HttpServerContext.Log.Debug
                    (
                        _internationalizationManager.Translate("webexpress:componentmanager.remove")
                    );
                }
            }

            _dictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Raises the AddComponent event.
        /// </summary>
        /// <param name="component">The component.</param>
        private void OnAddComponent(IComponentManager component)
        {
            AddComponent?.Invoke(null, component);
        }

        /// <summary>
        /// Raises the RemoveComponent event.
        /// </summary>
        /// <param name="component">The component.</param>
        private void OnRemoveComponent(IComponentManager component)
        {
            RemoveComponent?.Invoke(null, component);
        }

        /// <summary>
        /// Output of the components to the log.
        /// </summary>
        internal void LogStatus()
        {
            using var frame = new LogFrameSimple(HttpServerContext.Log);
            var output = new List<string>
            {
                _internationalizationManager.Translate("webexpress:componentmanager.component")
            };

            foreach (var pluginContext in PluginManager.Plugins)
            {
                output.Add
                (
                   string.Empty.PadRight(2) +
                   _internationalizationManager.Translate("webexpress:pluginmanager.plugin", pluginContext.PluginId)
                );

                _applicationManager.PrepareForLog(pluginContext, output, 4);
                _moduleManager.PrepareForLog(pluginContext, output, 4);
                _resourceManager.PrepareForLog(pluginContext, output, 4);
                _statusPageManager.PrepareForLog(pluginContext, output, 4);
                JobManager.PrepareForLog(pluginContext, output, 4);
            }

            //foreach (var item in Dictionary)
            //{
            //    foreach (var component in item.Value)
            //    {
            //        output.Add
            //        (
            //           string.Empty.PadRight(2) +
            //           I18N.Translate("webexpress:pluginmanager.plugin", item.Key.PluginId)
            //        );

            //        component.ComponentInstance?.PrepareForLog(item.Key, output, 4);
            //    }
            //}

            HttpServerContext.Log.Info(string.Join(Environment.NewLine, output));
        }
    }
}
