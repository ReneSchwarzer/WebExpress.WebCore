using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebEvent;
using WebExpress.WebCore.WebJob;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPackage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebResource;
using WebExpress.WebCore.WebSession;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebCore.WebStatusPage;
using WebExpress.WebCore.WebTask;

namespace WebExpress.WebCore.WebComponent
{
    /// <summary>
    /// Central management of components.
    /// </summary>
    public class ComponentManager : IComponentManager
    {
        private readonly InternationalizationManager _internationalizationManager;
        private readonly PluginManager _pluginManager;
        private readonly ApplicationManager _applicationManager;
        private readonly ModuleManager _moduleManager;
        private readonly ResourceManager _resourceManager;
        private readonly SitemapManager _sitemapManager;

        /// <summary>
        /// An event that fires when an component is added.
        /// </summary>
        public event EventHandler<IManager> AddComponent;

        /// <summary>
        /// An event that fires when an component is removed.
        /// </summary>
        public event EventHandler<IManager> RemoveComponent;

        /// <summary>
        /// Returns the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns the directory where the components are listed.
        /// </summary>
        private ComponentDictionary Dictionary { get; } = [];

        /// <summary>
        /// Returns all registered managers.
        /// </summary>
        public IEnumerable<IManager> Managers => new IManager[]
            {
                LogManager,
                PackageManager,
                _pluginManager,
                ApplicationManager,
                _moduleManager,
                EventManager,
                JobManager,
                StatusPageManager,
                _sitemapManager,
                _internationalizationManager,
                SessionManager,
                TaskManager
            }.Concat(Dictionary.Values.SelectMany(x => x).Select(x => x.ComponentInstance));

        /// <summary>
        /// Returns the log manager.
        /// </summary>
        /// <returns>The instance of the log manager or null.</returns>
        public LogManager LogManager { get; private set; }

        /// <summary>
        /// Returns the package manager.
        /// </summary>
        /// <returns>The instance of the package manager or null.</returns>
        public PackageManager PackageManager { get; private set; }

        /// <summary>
        /// Returns the plugin manager.
        /// </summary>
        /// <returns>The instance of the plugin manager or null.</returns>
        public IPluginManager PluginManager => _pluginManager;

        /// <summary>
        /// Returns the application manager.
        /// </summary>
        /// <returns>The instance of the application manager or null.</returns>
        public IApplicationManager ApplicationManager => _applicationManager;

        /// <summary>
        /// Returns the module manager.
        /// </summary>
        /// <returns>The instance of the module manager or null.</returns>
        public IModuleManager ModuleManager => _moduleManager;

        /// <summary>
        /// Returns the event manager.
        /// </summary>
        /// <returns>The instance of the event manager or null.</returns>
        public EventManager EventManager { get; private set; }

        /// <summary>
        /// Returns the job manager.
        /// </summary>
        /// <returns>The instance of the job manager or null.</returns>
        public JobManager JobManager { get; private set; }

        /// <summary>
        /// Returns the status page manager.
        /// </summary>
        /// <returns>The instance of the status page manager or null.</returns>
        public StatusPageManager StatusPageManager { get; private set; }

        /// <summary>
        /// Returns the resource manager.
        /// </summary>
        /// <returns>The instance of the resource manager or null.</returns>
        public IResourceManager ResourceManager => _resourceManager;

        /// <summary>
        /// Returns the sitemap manager.
        /// </summary>
        /// <returns>The instance of the sitemap manager or null.</returns>
        public ISitemapManager SitemapManager => _sitemapManager;

        /// <summary>
        /// Returns the internationalization manager.
        /// </summary>
        /// <returns>The instance of the internationalization manager or null.</returns>
        public IInternationalizationManager InternationalizationManager => _internationalizationManager;

        /// <summary>
        /// Returns the session manager.
        /// </summary>
        /// <returns>The instance of the session manager or null.</returns>
        public SessionManager SessionManager { get; private set; }

        /// <summary>
        /// Returns the task manager.
        /// </summary>
        /// <returns>The instance of the task manager manager or null.</returns>
        public TaskManager TaskManager { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        internal ComponentManager(IHttpServerContext httpServerContext)
        {
            HttpServerContext = httpServerContext;

            // order is relevant
            LogManager = CreateInstance(typeof(LogManager)) as LogManager;
            PackageManager = CreateInstance(typeof(PackageManager)) as PackageManager;
            _pluginManager = CreateInstance(typeof(PluginManager)) as PluginManager;
            _internationalizationManager = CreateInstance(typeof(InternationalizationManager)) as InternationalizationManager;
            _applicationManager = CreateInstance(typeof(ApplicationManager)) as ApplicationManager;
            _moduleManager = CreateInstance(typeof(ModuleManager)) as ModuleManager;
            _resourceManager = CreateInstance(typeof(ResourceManager)) as ResourceManager;
            StatusPageManager = CreateInstance(typeof(StatusPageManager)) as StatusPageManager;
            EventManager = CreateInstance(typeof(EventManager)) as EventManager;
            JobManager = CreateInstance(typeof(JobManager)) as JobManager;
            _sitemapManager = CreateInstance(typeof(SitemapManager)) as SitemapManager;
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
        private IManager CreateInstance(Type componentType)
        {
            if (componentType == null)
            {
                return null;
            }
            else if (!componentType.GetInterfaces().Where(x => x == typeof(IManager)).Any())
            {
                HttpServerContext.Log.Warning
                (
                    _internationalizationManager.Translate
                    (
                        "webexpress:componentmanager.wrongtype",
                        componentType?.FullName, typeof(IManager).FullName
                    )
                );

                return null;
            }

            try
            {
                return ComponentActivator.CreateInstance<IManager>(componentType, this);
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
        public IManager GetComponent(string id)
        {
            return Dictionary.Values
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
        public T GetComponent<T>() where T : IManager
        {
            return (T)Dictionary.Values
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
            if (Dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            var assembly = pluginContext.Assembly;

            Dictionary.Add(pluginContext, []);
            var componentItems = Dictionary[pluginContext];

            foreach (var type in assembly.GetExportedTypes().Where(x => x.IsClass && x.IsSealed && x.GetInterface(typeof(IManager).Name) != null))
            {
                var id = type.FullName?.ToLower();

                // determining attributes
                var componentInstance = CreateInstance(type);

                if (!componentItems.Where(x => x.ComponentId.Equals(id, StringComparison.OrdinalIgnoreCase)).Any())
                {
                    componentItems.Add(new ComponentItem()
                    {
                        ComponentClass = type,
                        ComponentId = id,
                        ComponentInstance = componentInstance
                    });

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

            foreach (var component in Dictionary.Values
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

            foreach (var component in Dictionary.Values
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

            if (Dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            var componentItems = Dictionary[pluginContext];

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

            _moduleManager.Remove(pluginContext);
            _applicationManager.Remove(pluginContext);
            _pluginManager.Remove(pluginContext);

            Dictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Raises the AddComponent event.
        /// </summary>
        /// <param name="component">The component.</param>
        private void OnAddComponent(IManager component)
        {
            AddComponent?.Invoke(null, component);
        }

        /// <summary>
        /// Raises the RemoveComponent event.
        /// </summary>
        /// <param name="component">The component.</param>
        private void OnRemoveComponent(IManager component)
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
                StatusPageManager.PrepareForLog(pluginContext, output, 4);
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
