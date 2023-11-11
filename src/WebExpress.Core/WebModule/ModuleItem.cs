using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebExpress.Core.Internationalization;
using WebExpress.Core.WebApplication;
using WebExpress.Core.WebPlugin;
using WebExpress.Core.WebUri;

namespace WebExpress.Core.WebModule
{
    /// <summary>
    /// Represents a module entry in the module directory.
    /// </summary>
    internal class ModuleItem : IDisposable
    {
        /// <summary>
        /// The assembly that contains the module.
        /// </summary>
        public Assembly Assembly { get; internal set; }

        /// <summary>
        /// Returns the module class.
        /// </summary>
        public Type ModuleClass { get; internal set; }

        /// <summary>
        /// Returns the context of the associated plugin.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the associated application ids.
        /// </summary>
        public IEnumerable<string> Applications { get; set; }

        /// <summary>
        /// Returns the modul id.
        /// </summary>
        public string ModuleId { get; internal set; }

        /// <summary>
        /// Returns the module name.
        /// </summary>
        public string ModuleName { get; internal set; }

        /// <summary>
        /// Returns the description.
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Returns the asset directory.
        /// </summary>
        public string AssetPath { get; internal set; }

        /// <summary>
        /// Returns the data directory. 
        /// </summary>
        public string DataPath { get; internal set; }

        /// <summary>
        /// Returns the context path.
        /// </summary>
        public UriResource ContextPath { get; internal set; }

        /// <summary>
        /// Returns the icon uri.
        /// </summary>
        public UriResource Icon { get; internal set; }

        /// <summary>
        /// Returns the log to write status messages to the console and to a log file.
        /// </summary>
        public Log Log { get; internal set; }

        /// <summary>
        /// Returns the directory where the module instances are listed.
        /// </summary>
        public IDictionary<IApplicationContext, ModuleItemInstance> Dictionary { get; }
            = new Dictionary<IApplicationContext, ModuleItemInstance>();

        /// <summary>
        /// An event that fires when an module is added.
        /// </summary>
        public event EventHandler<IModuleContext> AddModule;

        /// <summary>
        /// An event that fires when an module is removed.
        /// </summary>
        public event EventHandler<IModuleContext> RemoveModule;

        /// <summary>
        /// Adds an application assignment
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        public void AddApplication(IApplicationContext applicationContext)
        {
            // only if no instance has been created yet
            if (Dictionary.ContainsKey(applicationContext))
            {
                return;
            }

            // create context
            var moduleContext = new ModuleContext()
            {
                PluginContext = PluginContext,
                ApplicationContext = applicationContext,
                ModuleId = ModuleId,
                ModuleName = ModuleName,
                Description = Description,
                Icon = UriResource.Combine(applicationContext.ContextPath, ContextPath, Icon),
                AssetPath = Path.Combine(applicationContext.AssetPath, AssetPath),
                DataPath = Path.Combine(applicationContext.DataPath, DataPath),
                ContextPath = UriResource.Combine(applicationContext.ContextPath, ContextPath)
            };

            if
            (
                Applications.Contains("*") ||
                Applications.Contains(applicationContext.ApplicationId?.ToLower()) ||
                Applications.Where(x => Regex.Match(applicationContext.ApplicationId?.ToLower(), x).Success).Any()
            )
            {
                Dictionary.Add
                (
                    applicationContext,
                    new ModuleItemInstance()
                    {
                        ModuleContext = moduleContext
                    }
                );

                // raises the AddModule event
                OnAddModule(moduleContext);
            }
        }

        /// <summary>
        /// Remove an application assignment
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        public void DetachApplication(IApplicationContext applicationContext)
        {
            // not an instance has been created yet
            if (!Dictionary.ContainsKey(applicationContext))
            {
                return;
            }

            var moduleItemValue = Dictionary[applicationContext];
            OnRemoveModule(moduleItemValue.ModuleContext);

            Dictionary.Remove(applicationContext);
        }

        /// <summary>
        /// Boots the module of each existing application if not yet booted.
        /// </summary>
        public void Boot()
        {
            foreach (var item in Dictionary.Values.Where(x => x.ModuleInstance == null))
            {
                // create module
                item.ModuleInstance = Activator.CreateInstance(ModuleClass) as IModule;

                // thread termination.
                var token = item.CancellationTokenSource.Token;

                // initialize module
                item.ModuleInstance.Initialization(item.ModuleContext);

                Log.Debug
                (
                    message: InternationalizationManager.I18N
                    (
                        "webexpress:modulemanager.module.initialization",
                        item.ModuleContext.ApplicationContext.ApplicationId,
                        item.ModuleContext.PluginContext.PluginId
                    )
                );

                // execute modules concurrently
                Task.Run(() =>
                {
                    Log.Debug
                    (
                        message: InternationalizationManager.I18N
                        (
                            "webexpress:modulemanager.module.processing.start",
                            item.ModuleContext.ApplicationContext.ApplicationId,
                            item.ModuleContext.PluginContext.PluginId
                        )
                    );

                    item.ModuleInstance.Run();

                    Log.Debug
                    (
                        message: InternationalizationManager.I18N
                        (
                            "webexpress:modulemanager.module.processing.end",
                            item.ModuleContext.ApplicationContext.ApplicationId,
                            item.ModuleContext.PluginContext.PluginId
                        )
                    );

                    token.ThrowIfCancellationRequested();
                }, token);
            }
        }

        /// <summary>
        /// Terminate modules of a plugin.
        /// </summary>
        public void ShutDown()
        {
            foreach (var item in Dictionary.Values)
            {
                item.CancellationTokenSource?.Cancel();
            }
        }

        /// <summary>
        /// Terminate modules of a application.
        /// </summary>
        /// <param name="pluginContext">The context of the application containing the modules.</param>
        public void ShutDown(IApplicationContext applicationContext)
        {
            if (Dictionary.ContainsKey(applicationContext)
                && Dictionary[applicationContext].CancellationTokenSource != null)
            {
                Dictionary[applicationContext].CancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        /// Raises the AddModule event.
        /// </summary>
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
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (Delegate d in AddModule.GetInvocationList())
            {
                AddModule -= (EventHandler<IModuleContext>)d;
            }

            foreach (Delegate d in RemoveModule.GetInvocationList())
            {
                RemoveModule -= (EventHandler<IModuleContext>)d;
            }
        }

        /// <summary>
        /// Convert the module element to a string.
        /// </summary>
        /// <returns>The resource element in its string representation.</returns>
        public override string ToString()
        {
            return $"Module '{ModuleId}'";
        }
    }
}
