using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebModule
{
    /// <summary>
    /// The interface of the module manager.
    /// </summary>
    public interface IModuleManager : IComponentManager
    {
        /// <summary>
        /// An event that fires when an module is added.
        /// </summary>
        event EventHandler<IModuleContext> AddModule;

        /// <summary>
        /// An event that fires when an module is removed.
        /// </summary>
        event EventHandler<IModuleContext> RemoveModule;

        /// <summary>
        /// Returns all stored modules.
        /// </summary>
        public IEnumerable<IModuleContext> Modules { get; }

        /// <summary>
        /// Determines the module for a given application context and module id.
        /// </summary>
        /// <param name="applicationType">The type of the application.</param>
        /// <param name="moduleId">The type of the module.</param>
        /// <returns>The context of the module or null.</returns>
        public IModuleContext GetModule(Type applicationType, Type moduleType);

        /// <summary>
        /// Determines the module for a given application context and module id.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="moduleId">The modul id.</param>
        /// <returns>The context of the module or null.</returns>
        public IModuleContext GetModule(IApplicationContext applicationContext, string moduleId);

        /// <summary>
        /// Determines the module for a given application context and module id.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="moduleClass">The module class.</param>
        /// <returns>The context of the module or null.</returns>
        public IModuleContext GetModule(IApplicationContext applicationContext, Type moduleClass);

        /// <summary>
        /// Determines the module for a given plugin context and module id.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="moduleId">The modul id.</param>
        /// <returns>An enumeration of the module contexts for the given plugin and module id.</returns>
        public IEnumerable<IModuleContext> GetModules(IPluginContext pluginContext, string moduleId);

        /// <summary>
        /// Determines the module for a given plugin context and module id.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="moduleId">The modul id.</param>
        /// <returns>An enumeration of the module contexts for the given plugin and module id.</returns>
        public IEnumerable<IModuleContext> GetModules(IApplicationContext applicationContext);
    }
}
