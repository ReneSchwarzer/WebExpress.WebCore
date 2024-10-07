using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebModule
{
    /// <summary>
    /// The module manager manages the WebExpress modules.
    /// </summary>
    public sealed class ModuleManager : IModuleManager, IComponentManagerPlugin, IExecutableElements, ISystemComponent
    {
        private readonly IComponentHub _componentManager;
        private readonly IHttpServerContext _httpServerContext;
        private readonly ModuleDictionary _dictionary = [];

        /// <summary>
        /// An event that fires when an module is added.
        /// </summary>
        public event EventHandler<IModuleContext> AddModule;

        /// <summary>
        /// An event that fires when an module is removed.
        /// </summary>
        public event EventHandler<IModuleContext> RemoveModule;

        /// <summary>
        /// Delivers all stored modules.
        /// </summary>
        public IEnumerable<IModuleContext> Modules => _dictionary.Values
            .SelectMany(x => x.Values)
            .SelectMany(x => x.Dictionary.Values)
            .Select(x => x.ModuleContext);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentManager">The component manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private ModuleManager(IComponentHub componentManager, IHttpServerContext httpServerContext)
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

            _componentManager.ApplicationManager.AddApplication += (sender, applicationContext) =>
            {
                AssignToApplication(applicationContext);
            };

            _componentManager.ApplicationManager.RemoveApplication += (sender, applicationContext) =>
            {
                DetachFromApplication(applicationContext);
            };

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress:modulemanager.initialization")
            );
        }

        /// <summary>
        /// Discovers and registers modules from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose modules are to be registered.</param>
        public void Register(IPluginContext pluginContext)
        {
            if (_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            var assembly = pluginContext.Assembly;

            foreach (var type in assembly.GetExportedTypes().Where
                (
                    x => x.IsClass &&
                    x.IsSealed &&
                    x.IsPublic &&
                    x.GetInterface(typeof(IModule).Name) != null
                ))
            {
                var id = type.FullName?.ToLower();
                var name = type.Name.ToLower();
                var icon = string.Empty;
                var description = string.Empty;
                var contextPath = string.Empty;
                var assetPath = string.Empty;
                var dataPath = string.Empty;
                var applicationIds = new List<string>();

                foreach (var customAttribute in type.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces()
                    .Contains(typeof(IModuleAttribute))))
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
                    else if (customAttribute.AttributeType == typeof(ContextPathAttribute))
                    {
                        contextPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(AssetPathAttribute))
                    {
                        assetPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(DataPathAttribute))
                    {
                        dataPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(ApplicationAttribute))
                    {
                        applicationIds.Add(customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString().Trim());
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ApplicationAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ApplicationAttribute<>).Namespace)
                    {
                        applicationIds.Add(customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower());
                    }
                }

                if (!applicationIds.Any())
                {
                    // no application specified
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate("webexpress:modulemanager.applicationless", id)
                    );
                }

                _dictionary.TryAdd(pluginContext, new Dictionary<string, ModuleItem>());
                var item = _dictionary[pluginContext];

                if (!item.ContainsKey(id))
                {
                    var moduleItem = new ModuleItem()
                    {
                        Assembly = assembly,
                        ModuleClass = type,
                        PluginContext = pluginContext,
                        Applications = applicationIds,
                        ModuleId = id,
                        ModuleName = name,
                        Description = description,
                        Icon = new UriResource(icon),
                        AssetPath = assetPath,
                        ContextPath = new UriResource(contextPath),
                        DataPath = dataPath,
                        Log = _httpServerContext.Log
                    };

                    moduleItem.AddModule += (s, e) =>
                    {
                        OnAddModule(e);
                    };

                    moduleItem.RemoveModule += (s, e) =>
                    {
                        OnRemoveModule(e);
                    };

                    item.Add(id, moduleItem);

                    // assign the module to existing applications.
                    foreach (var applicationContext in _componentManager.ApplicationManager.Applications)
                    {
                        AssignToApplication(applicationContext);
                    }

                    _httpServerContext.Log.Debug
                    (
                        I18N.Translate
                        (
                            "webexpress:modulemanager.register",
                            id,
                            string.Join(", ", applicationIds)
                        )
                    );
                }
                else
                {
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate
                        (
                            "webexpress:modulemanager.duplicate",
                            id,
                            string.Join(", ", applicationIds)
                        )
                    );
                }
            }
        }

        /// <summary>
        /// Discovers and registers modules from the specified plugin.
        /// </summary>
        /// <param name="pluginContexts">A list with plugin contexts that contain the modules.</param>
        public void Register(IEnumerable<IPluginContext> pluginContexts)
        {
            foreach (var pluginContext in pluginContexts)
            {
                Register(pluginContext);
            }
        }

        /// <summary>
        /// Assign existing modules to the application.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        private void AssignToApplication(IApplicationContext applicationContext)
        {
            foreach (var moduleItem in _dictionary.Values.SelectMany(x => x.Values))
            {
                if (moduleItem.Applications.Contains("*")
                    || moduleItem.Applications.Contains(applicationContext?.ApplicationId?.ToLower()))
                {
                    moduleItem.AddApplication(applicationContext);
                }
            }
        }

        /// <summary>
        /// Remove an existing modules to the application.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        private void DetachFromApplication(IApplicationContext applicationContext)
        {
            foreach (var moduleItem in _dictionary.Values.SelectMany(x => x.Values))
            {
                if (moduleItem.Applications.Contains("*")
                    || moduleItem.Applications.Contains(applicationContext?.ApplicationId?.ToLower()))
                {
                    moduleItem.DetachApplication(applicationContext);
                }
            }
        }

        /// <summary>
        /// Determines the module for a given application context and module id.
        /// </summary>
        /// <param name="applicationType">The type of the application.</param>
        /// <param name="moduleId">The type of the module.</param>
        /// <returns>The context of the module or null.</returns>
        public IModuleContext GetModule(Type applicationType, Type moduleType)
        {
            var applicationContext = _componentManager.ApplicationManager.GetApplcation(applicationType);
            var item = _dictionary.Values
                .SelectMany(x => x.Values)
                .Where(x => x.Dictionary.ContainsKey(applicationContext))
                .Where(x => x.ModuleClass.Equals(moduleType))
                .Select(x => x.Dictionary[applicationContext])
                .Select(x => x.ModuleContext)
                .FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Determines the module for a given application context and module id.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="moduleId">The modul id.</param>
        /// <returns>The context of the module or null.</returns>
        public IModuleContext GetModule(IApplicationContext applicationContext, string moduleId)
        {
            var item = _dictionary.Values
                .SelectMany(x => x.Values)
                .Where(x => x.Dictionary.ContainsKey(applicationContext))
                .Select(x => x.Dictionary[applicationContext])
                .Where(x => x.ModuleContext.ModuleId.Equals(moduleId, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.ModuleContext)
                .FirstOrDefault();

            return item;
        }

        /// <summary>
        /// Determines the module for a given application context and module id.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="moduleClass">The module class.</param>
        /// <returns>The context of the module or null.</returns>
        public IModuleContext GetModule(IApplicationContext applicationContext, Type moduleClass)
        {
            return GetModule(applicationContext, moduleClass.FullName.ToLower());
        }

        /// <summary>
        /// Determines the module for a given plugin context and module id.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="moduleId">The modul id.</param>
        /// <returns>An enumeration of the module contexts for the given plugin and module id.</returns>
        public IEnumerable<IModuleContext> GetModules(IPluginContext pluginContext, string moduleId)
        {
            return GetModuleItems(pluginContext)
                .Where(x => x.ModuleId.Equals(moduleId, StringComparison.OrdinalIgnoreCase))
                .SelectMany(x => x.Dictionary.Values)
                .Select(x => x.ModuleContext);
        }

        /// <summary>
        /// Determines the module for a given plugin context and module id.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="moduleId">The modul id.</param>
        /// <returns>An enumeration of the module contexts for the given plugin and module id.</returns>
        public IEnumerable<IModuleContext> GetModules(IApplicationContext applicationContext)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .Where(x => x.Dictionary.ContainsKey(applicationContext))
                .Select(x => x.Dictionary[applicationContext])
                .Select(x => x.ModuleContext);
        }

        /// <summary>
        /// Returns the modules for a given plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <returns>An enumeration of the module contexts for the given plugin.</returns>
        internal IEnumerable<ModuleItem> GetModuleItems(IPluginContext pluginContext)
        {
            if (pluginContext == null || !_dictionary.ContainsKey(pluginContext))
            {
                return [];
            }

            return _dictionary[pluginContext].Values;
        }

        /// <summary>
        /// Boots the modules of a plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin containing the modules.</param>
        public void Boot(IPluginContext pluginContext)
        {
            foreach (var moduleItem in GetModuleItems(pluginContext))
            {
                // initialize module
                moduleItem.Boot();
            }
        }

        /// <summary>
        /// Terminate modules of a plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin containing the modules.</param>
        public void ShutDown(IPluginContext pluginContext)
        {
            foreach (var moduleItem in GetModuleItems(pluginContext))
            {
                // terminate module
                moduleItem.ShutDown();
            }
        }

        /// <summary>
        /// Terminate modules of a application.
        /// </summary>
        /// <param name="pluginContext">The context of the application containing the modules.</param>
        public void ShutDown(IApplicationContext applicationContext)
        {
            foreach (var moduleItem in _dictionary.Values.SelectMany(x => x.Values))
            {
                // terminate module
                moduleItem.ShutDown(applicationContext);
            }
        }

        /// <summary>
        /// Removes all modules associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the modules to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            if (pluginContext == null)
            {
                return;
            }

            if (!_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            ShutDown(pluginContext);

            foreach (var moduleItem in _dictionary[pluginContext].Values)
            {
                moduleItem.Dispose();
            }

            _dictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Raises the AddModule event.
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        private void OnAddModule(IModuleContext moduleContext)
        {
            AddModule?.Invoke(this, moduleContext);
        }

        /// <summary>
        /// Raises the RemoveModule event.
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        private void OnRemoveModule(IModuleContext moduleContext)
        {
            RemoveModule?.Invoke(this, moduleContext);
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
            foreach (var moduleContext in GetModuleItems(pluginContext))
            {
                output.Add
                (
                    string.Empty.PadRight(deep) +
                    I18N.Translate
                    (
                        "webexpress:modulemanager.module",
                        moduleContext.ModuleId,
                        string.Join(",", moduleContext.Applications)
                    )
                );
            }
        }
    }
}
