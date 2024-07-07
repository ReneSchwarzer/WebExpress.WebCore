using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public static class ComponentManager
    {
        /// <summary>
        /// An event that fires when an component is added.
        /// </summary>
        public static event EventHandler<IComponent> AddComponent;

        /// <summary>
        /// An event that fires when an component is removed.
        /// </summary>
        public static event EventHandler<IComponent> RemoveComponent;

        /// <summary>
        /// Returns the reference to the context of the host.
        /// </summary>
        public static IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns the directory where the components are listed.
        /// </summary>
        private static ComponentDictionary Dictionary { get; } = new ComponentDictionary();

        /// <summary>
        /// Returns all registered components.
        /// </summary>
        public static IEnumerable<IComponent> Components => new IComponent[]
            {
                PackageManager,
                PluginManager,
                ApplicationManager,
                ModuleManager,
                EventManager,
                JobManager,
                ResponseManager,
                SitemapManager,
                InternationalizationManager,
                SessionManager,
                TaskManager
            }.Concat(Dictionary.Values.SelectMany(x => x).Select(x => x.ComponentInstance));

        /// <summary>
        /// Returns the log manager.
        /// </summary>
        /// <returns>The instance of the log manager or null.</returns>
        public static LogManager LogManager { get; private set; }

        /// <summary>
        /// Returns the package manager.
        /// </summary>
        /// <returns>The instance of the package manager or null.</returns>
        public static PackageManager PackageManager { get; private set; }

        /// <summary>
        /// Returns the plugin manager.
        /// </summary>
        /// <returns>The instance of the plugin manager or null.</returns>
        public static PluginManager PluginManager { get; private set; }

        /// <summary>
        /// Returns the application manager.
        /// </summary>
        /// <returns>The instance of the application manager or null.</returns>
        public static ApplicationManager ApplicationManager { get; private set; }

        /// <summary>
        /// Returns the module manager.
        /// </summary>
        /// <returns>The instance of the module manager or null.</returns>
        public static ModuleManager ModuleManager { get; private set; }

        /// <summary>
        /// Returns the event manager.
        /// </summary>
        /// <returns>The instance of the event manager or null.</returns>
        public static EventManager EventManager { get; private set; }

        /// <summary>
        /// Returns the job manager.
        /// </summary>
        /// <returns>The instance of the job manager or null.</returns>
        public static JobManager JobManager { get; private set; }

        /// <summary>
        /// Returns the response manager.
        /// </summary>
        /// <returns>The instance of the response manager or null.</returns>
        public static ResponseManager ResponseManager { get; private set; }

        /// <summary>
        /// Returns the resource manager.
        /// </summary>
        /// <returns>The instance of the resource manager or null.</returns>
        public static ResourceManager ResourceManager { get; private set; }

        /// <summary>
        /// Returns the sitemap manager.
        /// </summary>
        /// <returns>The instance of the sitemap manager or null.</returns>
        public static SitemapManager SitemapManager { get; private set; }

        /// <summary>
        /// Returns the internationalization manager.
        /// </summary>
        /// <returns>The instance of the internationalization manager or null.</returns>
        public static InternationalizationManager InternationalizationManager { get; private set; }

        /// <summary>
        /// Returns the session manager.
        /// </summary>
        /// <returns>The instance of the session manager or null.</returns>
        public static SessionManager SessionManager { get; private set; }

        /// <summary>
        /// Returns the task manager.
        /// </summary>
        /// <returns>The instance of the task manager manager or null.</returns>
        public static TaskManager TaskManager { get; private set; }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        internal static void Initialization(IHttpServerContext httpServerContext)
        {
            HttpServerContext = httpServerContext;

            InternationalizationManager.Register(typeof(HttpServer).Assembly, "webexpress");

            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N("webexpress:componentmanager.initialization")
            );

            // order is relevant
            LogManager = CreateInstance(typeof(LogManager)) as LogManager;
            PackageManager = CreateInstance(typeof(PackageManager)) as PackageManager;
            PluginManager = CreateInstance(typeof(PluginManager)) as PluginManager;
            InternationalizationManager = CreateInstance(typeof(InternationalizationManager)) as InternationalizationManager;
            ApplicationManager = CreateInstance(typeof(ApplicationManager)) as ApplicationManager;
            ModuleManager = CreateInstance(typeof(ModuleManager)) as ModuleManager;
            ResourceManager = CreateInstance(typeof(ResourceManager)) as ResourceManager;
            ResponseManager = CreateInstance(typeof(ResponseManager)) as ResponseManager;
            EventManager = CreateInstance(typeof(EventManager)) as EventManager;
            JobManager = CreateInstance(typeof(JobManager)) as JobManager;
            SitemapManager = CreateInstance(typeof(SitemapManager)) as SitemapManager;
            SessionManager = CreateInstance(typeof(SessionManager)) as SessionManager;
            TaskManager = CreateInstance(typeof(TaskManager)) as TaskManager;

            PluginManager.AddPlugin += (sender, pluginContext) =>
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
        private static IComponent CreateInstance(Type componentType)
        {
            if (componentType == null)
            {
                return null;
            }
            else if (!componentType.GetInterfaces().Where(x => x == typeof(IComponent)).Any())
            {
                HttpServerContext.Log.Warning
                (
                    InternationalizationManager.I18N
                    (
                        "webexpress:componentmanager.wrongtype",
                        componentType?.FullName, typeof(IComponent).FullName
                    )
                );

                return null;
            }

            try
            {
                var flags = BindingFlags.NonPublic | BindingFlags.Instance;
                var component = componentType?.Assembly.CreateInstance
                (
                    componentType?.FullName,
                    false,
                    flags,
                    null,
                    null,
                    null,
                    null
                ) as IComponent;

                component.Initialization(HttpServerContext);

                return component;
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
        public static IComponent GetComponent(string id)
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
        public static T GetComponent<T>() where T : IComponent
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
        public static void Register(IPluginContext pluginContext)
        {
            // the plugin has already been registered
            if (Dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            var assembly = pluginContext.Assembly;

            Dictionary.Add(pluginContext, new List<ComponentItem>());
            var componentItems = Dictionary[pluginContext];

            foreach (var type in assembly.GetExportedTypes().Where(x => x.IsClass && x.IsSealed && x.GetInterface(typeof(IComponent).Name) != null))
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
                        InternationalizationManager.I18N("webexpress:componentmanager.register", id)
                    );

                    // raises the AddComponent event
                    OnAddComponent(componentInstance);
                }
                else
                {
                    HttpServerContext.Log.Warning
                    (
                        InternationalizationManager.I18N("webexpress:componentmanager.duplicate", id)
                    );
                }
            }
        }

        /// <summary>
        /// Discovers and registers the components from the specified plugins.
        /// </summary>
        /// <param name="pluginContexts">A list with plugin contexts that contain the components.</param>
        public static void Register(IEnumerable<IPluginContext> pluginContexts)
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
        internal static void BootComponent(IPluginContext pluginContext)
        {
            PluginManager.Boot(pluginContext);
            ApplicationManager.Boot(pluginContext);
            ModuleManager.Boot(pluginContext);

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
        internal static void BootComponent(IEnumerable<IPluginContext> pluginContexts)
        {
            foreach (var pluginContext in pluginContexts)
            {
                BootComponent(pluginContext);
            }
        }

        /// <summary>
        /// Starts the component.
        /// </summary>
        internal static void Execute()
        {
            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N("webexpress:componentmanager.execute")
            );

            PackageManager.Execute();
            JobManager.Execute();
        }

        /// <summary>
        /// Shutting down the component manager.
        /// </summary>
        public static void ShutDown()
        {
            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N("webexpress:componentmanager.shutdown")
            );
        }

        /// <summary>
        /// Shutting down the component.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        public static void ShutDownComponent(IPluginContext pluginContext)
        {
            PluginManager.ShutDown(pluginContext);
            ApplicationManager.ShutDown(pluginContext);
            ModuleManager.ShutDown(pluginContext);

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
        public static void Remove(IPluginContext pluginContext)
        {
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
                    InternationalizationManager.I18N("webexpress:componentmanager.remove")
                );
            }

            ModuleManager.Remove(pluginContext);
            ApplicationManager.Remove(pluginContext);
            PluginManager.Remove(pluginContext);

            Dictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Raises the AddComponent event.
        /// </summary>
        /// <param name="component">The component.</param>
        private static void OnAddComponent(IComponent component)
        {
            AddComponent?.Invoke(null, component);
        }

        /// <summary>
        /// Raises the RemoveComponent event.
        /// </summary>
        /// <param name="component">The component.</param>
        private static void OnRemoveComponent(IComponent component)
        {
            RemoveComponent?.Invoke(null, component);
        }

        /// <summary>
        /// Output of the components to the log.
        /// </summary>
        public static void LogStatus()
        {
            using var frame = new LogFrameSimple(HttpServerContext.Log);
            var output = new List<string>
            {
                InternationalizationManager.I18N("webexpress:componentmanager.component")
            };

            foreach (var pluginContext in PluginManager.Plugins)
            {
                output.Add
                (
                   string.Empty.PadRight(2) +
                   InternationalizationManager.I18N("webexpress:pluginmanager.plugin", pluginContext.PluginId)
                );

                ApplicationManager.PrepareForLog(pluginContext, output, 4);
                ModuleManager.PrepareForLog(pluginContext, output, 4);
                ResourceManager.PrepareForLog(pluginContext, output, 4);
                ResponseManager.PrepareForLog(pluginContext, output, 4);
                JobManager.PrepareForLog(pluginContext, output, 4);
            }

            foreach (var item in Dictionary)
            {
                foreach (var component in item.Value)
                {
                    output.Add
                    (
                       string.Empty.PadRight(2) +
                       InternationalizationManager.I18N("webexpress:pluginmanager.plugin", item.Key.PluginId)
                    );

                    component.ComponentInstance?.PrepareForLog(item.Key, output, 4);
                }
            }

            HttpServerContext.Log.Info(string.Join(Environment.NewLine, output));
        }
    }
}
