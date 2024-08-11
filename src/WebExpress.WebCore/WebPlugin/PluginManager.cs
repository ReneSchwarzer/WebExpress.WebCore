using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPlugin
{
    /// <summary>
    /// The plugin manager manages the WebExpress plugins.
    /// </summary>
    public class PluginManager : IComponent, IExecutableElements, ISystemComponent
    {
        /// <summary>
        /// An event that fires when an plugin is added.
        /// </summary>
        public event EventHandler<IPluginContext> AddPlugin;

        /// <summary>
        /// An event that fires when an plugin is removed.
        /// </summary>
        public event EventHandler<IPluginContext> RemovePlugin;

        /// <summary>
        /// Returns or sets the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns the directory where the plugins are listed.
        /// </summary>
        private PluginDictionary Dictionary { get; } = [];

        /// <summary>
        /// Plugins that do not meet the dependencies.
        /// </summary>
        private PluginDictionary UnfulfilledDependencies { get; } = [];

        /// <summary>
        /// Returns all plugins.
        /// </summary>
        public IEnumerable<IPluginContext> Plugins => Dictionary.Values.Select(x => x.PluginContext).ToList();

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        internal PluginManager()
        {
            ComponentManager.AddComponent += (s, e) =>
            {
                //AssignToComponent(e);
            };

            ComponentManager.RemoveComponent += (s, e) =>
            {
                //DetachFromcomponent(e);
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
                InternationalizationManager.I18N("webexpress:pluginmanager.initialization")
            );
        }

        /// <summary>
        /// Loads and registers the plugins that are static (i.e. located in the application's folder).
        /// </summary>1
        /// <returns>A list of plugins created.</returns>
        internal void Register()
        {
            var path = Environment.CurrentDirectory;
            var assemblies = new List<Assembly>();

            // create plugins
            foreach (var assemblyFile in Directory.EnumerateFiles(path, "*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(assemblyFile);
                    if (assembly != null)
                    {
                        assemblies.Add(assembly);
                        HttpServerContext.Log.Debug
                        (
                            InternationalizationManager.I18N
                            (
                                "webexpress:pluginmanager.load",
                                assembly.GetName().Name,
                                assembly.GetName().Version.ToString()
                            )
                        );
                    }
                }
                catch (BadImageFormatException)
                {

                }
            }

            // register plugin
            foreach (var assembly in assemblies
                .OrderBy(x => x.GetCustomAttribute(typeof(SystemPluginAttribute)) != null ? 0 : 1))
            {
                Register(assembly);
            }

            Logging();
        }

        /// <summary>
        /// Loads and registers the plugins from a path.
        /// </summary>
        /// <param name="pluginFile">The directory and filename where the plugins are located.</param>
        internal IEnumerable<IPluginContext> Register(string pluginFile)
        {
            var assemblies = new List<Assembly>();
            var pluginContexts = new List<IPluginContext>();

            if (!File.Exists(pluginFile))
            {
                return pluginContexts;
            }

            var loadContext = new PluginLoadContext(pluginFile);

            // create plugins
            try
            {
                var assembly = loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginFile));

                if (assembly != null)
                {
                    assemblies.Add(assembly);
                    HttpServerContext.Log.Debug
                    (
                        InternationalizationManager.I18N
                        (
                            "webexpress:pluginmanager.load",
                            assembly.GetName().Name,
                            assembly.GetName().Version.ToString()
                        )
                    );
                }
            }
            catch (BadImageFormatException)
            {

            }

            // register plugin
            foreach (var assembly in assemblies)
            {
                var pluginContext = Register(assembly, loadContext);
                pluginContexts.Add(pluginContext);
            }

            Logging();

            return pluginContexts;
        }

        /// <summary>
        /// Loads and registers the plugin from an assembly.
        /// </summary>
        /// <param name="assembly">The assembly where the plugin is located.</param>
        /// <param name="loadContext">The plugin load context for isolating and unloading the dependent libraries.</param>
        /// <returns>A plugin created or null.</returns>
        private IPluginContext Register(Assembly assembly, PluginLoadContext loadContext = null)
        {
            try
            {
                foreach (var type in assembly
                    .GetExportedTypes()
                    .Where(x => x.IsClass && x.IsSealed)
                    .Where(x => x.GetInterface(typeof(IPlugin).Name) != null))
                {
                    var id = type.Namespace?.ToLower();
                    var name = type.Assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
                    var icon = string.Empty;
                    var description = type.Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
                    var dependencies = new List<string>();
                    var hasUnfulfilledDependencies = false;

                    foreach (var customAttribute in type.CustomAttributes
                        .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IPluginAttribute))))
                    {
                        if (customAttribute.AttributeType == typeof(NameAttribute))
                        {
                            name = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                        }
                        else if (customAttribute.AttributeType == typeof(IconAttribute))
                        {
                            icon = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                        }
                        else if (customAttribute.AttributeType == typeof(DescriptionAttribute))
                        {
                            description = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                        }
                        else if (customAttribute.AttributeType == typeof(DependencyAttribute))
                        {
                            dependencies.Add(customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString());
                        }
                    }

                    var pluginContext = new PluginContext()
                    {
                        Assembly = type.Assembly,
                        PluginId = id,
                        PluginName = name,
                        Manufacturer = type.Assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company,
                        Copyright = type.Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright,
                        Icon = UriResource.Combine(HttpServerContext?.ContextPath, icon),
                        Description = description,
                        Version = type.Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
                        Host = HttpServerContext
                    };

                    hasUnfulfilledDependencies = HasUnfulfilledDependencies(id, dependencies);

                    if (hasUnfulfilledDependencies)
                    {
                        UnfulfilledDependencies.Add(id, new PluginItem()
                        {
                            PluginLoadContext = loadContext,
                            PluginClass = type,
                            PluginContext = pluginContext,
                            Plugin = Activator.CreateInstance(type) as IPlugin,
                            Dependencies = dependencies
                        });
                    }
                    else if (!Dictionary.ContainsKey(id))
                    {
                        Dictionary.Add(id, new PluginItem()
                        {
                            PluginLoadContext = loadContext,
                            PluginClass = type,
                            PluginContext = pluginContext,
                            Plugin = Activator.CreateInstance(type) as IPlugin,
                            Dependencies = dependencies
                        });

                        HttpServerContext.Log.Debug
                        (
                            InternationalizationManager.I18N("webexpress:pluginmanager.created", id)
                        );

                        OnAddPlugin(pluginContext);

                        CheckUnfulfilledDependencies();
                    }
                    else
                    {
                        HttpServerContext.Log.Warning
                        (
                            InternationalizationManager.I18N("webexpress:pluginmanager.duplicate", id)
                        );
                    }

                    return pluginContext;
                }
            }
            catch (Exception ex)
            {
                HttpServerContext.Log.Exception(ex);
            }

            return null;
        }

        /// <summary>
        /// Removes all elemets associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the elemets to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            OnRemovePlugin(pluginContext);

            var pluginItem = GetPluginItem(pluginContext);
            pluginItem?.PluginLoadContext?.Unload();
        }

        /// <summary>
        /// Check if dependencies of other plugins are now fulfilled after a plugin has been added.
        /// </summary>
        private void CheckUnfulfilledDependencies()
        {
            bool fulfilledDependencies;

            do
            {
                fulfilledDependencies = false;

                foreach (var unfulfilledDependencies in UnfulfilledDependencies)
                {
                    var hasUnfulfilledDependencies = HasUnfulfilledDependencies
                    (
                        unfulfilledDependencies.Key,
                        unfulfilledDependencies.Value.Dependencies
                    );

                    if (!hasUnfulfilledDependencies)
                    {
                        fulfilledDependencies = true;
                        UnfulfilledDependencies.Remove(unfulfilledDependencies.Key);
                        Dictionary.Add(unfulfilledDependencies.Key, unfulfilledDependencies.Value);

                        OnAddPlugin(unfulfilledDependencies.Value.PluginContext);

                        HttpServerContext.Log.Debug
                        (
                            InternationalizationManager.I18N
                            (
                                "webexpress:pluginmanager.fulfilleddependencies",
                                unfulfilledDependencies.Key
                            )
                        );
                    }
                }
            } while (fulfilledDependencies);
        }

        /// <summary>
        /// Checks if there are any unfulfilled dependencies.
        /// </summary>
        /// <param name="id">The id of the plugin.</param>
        /// <param name="dependencies">The dependencies to check.</param>
        /// <returns>True if dependencies exist, false otherwise</returns>
        private bool HasUnfulfilledDependencies(string id, IEnumerable<string> dependencies)
        {
            var hasUnfulfilledDependencies = false;

            foreach (var dependency in dependencies
                   .Where(x => !Dictionary.ContainsKey(x.ToLower())))
            {
                // dependency was not fulfilled
                hasUnfulfilledDependencies = true;

                HttpServerContext.Log.Debug
                (
                    InternationalizationManager.I18N
                    (
                        "webexpress:pluginmanager.unfulfilleddependencies",
                        id,
                        dependency
                    )
                );
            }

            return hasUnfulfilledDependencies;
        }

        /// <summary>
        /// Returns a plugin context based on its id.
        /// </summary>
        /// <param name="pluginId">The id of the plugin.</param>
        /// <returns>The plugin context.</returns>
        public IPluginContext GetPlugin(string pluginId)
        {
            return Dictionary.Values
                .Where
                (
                    x => x.PluginContext != null &&
                    x.PluginContext.PluginId.Equals(pluginId, StringComparison.OrdinalIgnoreCase)
                )
                .Select(x => x.PluginContext)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns a plugin item based on the context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <returns>The plugin item or null.</returns>
        private PluginItem GetPluginItem(IPluginContext pluginContext)
        {
            var pluginId = pluginContext?.PluginId?.ToLower();

            if (pluginId == null || !Dictionary.TryGetValue(pluginId, out PluginItem value))
            {
                HttpServerContext.Log.Warning
                (
                    InternationalizationManager.I18N
                    (
                        "webexpress:pluginmanager.notavailable",
                        pluginId
                    )
                );

                return null;
            }

            return value;
        }

        /// <summary>
        /// Boots the specified plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin to run.</param>
        internal void Boot(IPluginContext pluginContext)
        {
            var pluginItem = GetPluginItem(pluginContext);
            var token = pluginItem?.CancellationTokenSource.Token;

            if (pluginItem == null)
            {
                return;
            }

            // initialize plugin
            pluginItem.Plugin.Initialization(pluginItem.PluginContext);
            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N
                (
                    "webexpress:pluginmanager.plugin.initialization",
                    pluginItem.PluginContext.PluginId
                )
            );

            // run plugin concurrently
            Task.Run(() =>
            {
                HttpServerContext.Log.Debug
                (
                    InternationalizationManager.I18N
                    (
                        "webexpress:pluginmanager.plugin.processing.start",
                        pluginItem.PluginContext.PluginId
                    )
                );

                pluginItem.Plugin.Run();

                HttpServerContext.Log.Debug
                (
                    InternationalizationManager.I18N
                    (
                        "webexpress:pluginmanager.plugin.processing.end",
                        pluginItem.PluginContext.PluginId
                    )
                );

                token?.ThrowIfCancellationRequested();
            }, token.Value);
        }

        /// <summary>
        /// Boots the specified plugins.
        /// </summary>
        /// <param name="contexts">A list with the contexts of the plugins to run.</param>
        internal void Boot(IEnumerable<IPluginContext> contexts)
        {
            foreach (var context in contexts)
            {
                Boot(context);
            }
        }

        /// <summary>
        /// Shut down the plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin to shut down.</param>
        internal void ShutDown(IPluginContext pluginContext)
        {
            var plugin = GetPluginItem(pluginContext);

            plugin?.CancellationTokenSource.Cancel();

        }

        /// <summary>
        /// Shut down the plugins.
        /// </summary>
        /// <param name="contexts">A list of contexts of plugins to shut down.</param>
        internal void ShutDown(IEnumerable<IPluginContext> contexts)
        {
            foreach (var context in contexts)
            {
                ShutDown(context);
            }
        }

        /// <summary>
        /// Raises the AddPlugin event.
        /// </summary>
        /// <param name="component">The plugin context.</param>
        private void OnAddPlugin(IPluginContext pluginContext)
        {
            AddPlugin?.Invoke(this, pluginContext);
        }

        /// <summary>
        /// Raises the RemovePlugin event.
        /// </summary>
        /// <param name="component">The plugin context.</param>
        private void OnRemovePlugin(IPluginContext pluginContext)
        {
            RemovePlugin?.Invoke(this, pluginContext);
        }

        /// <summary>
        /// Output of the loaded plugins to the log.
        /// </summary>
        private void Logging()
        {
            using var frame = new LogFrameSimple(HttpServerContext.Log);
            var list = new List<string>();
            HttpServerContext.Log.Info
            (
                InternationalizationManager.I18N
                (
                    "webexpress:pluginmanager.pluginmanager.label"
                )
            );

            list.AddRange(Dictionary
                .Where
                (
                    x => x.Value.PluginClass.Assembly
                        .GetCustomAttribute(typeof(SystemPluginAttribute)) != null
                )
                .Select(x => InternationalizationManager.I18N
                (
                    "webexpress:pluginmanager.pluginmanager.system",
                    x.Key
                ))
            );

            list.AddRange(Dictionary
                .Where
                (
                    x => x.Value.PluginClass.Assembly
                        .GetCustomAttribute(typeof(SystemPluginAttribute)) == null
                )
                .Select(x => InternationalizationManager.I18N
                (
                    "webexpress:pluginmanager.pluginmanager.custom",
                    x.Key
                ))
            );

            list.AddRange(UnfulfilledDependencies
                .Select(x => InternationalizationManager.I18N
                (
                    "webexpress:pluginmanager.pluginmanager.unfulfilleddependencies",
                    x.Key
                ))
            );

            foreach (var item in list)
            {
                HttpServerContext.Log.Info(string.Join(Environment.NewLine, item));
            }
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
        }
    }
}
