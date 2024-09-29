using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebPlugin
{
    /// <summary>
    /// The plugin manager manages the WebExpress plugins.
    /// </summary>
    public interface IPluginManager : IManager
    {
        /// <summary>
        /// An event that fires when an plugin is added.
        /// </summary>
        event EventHandler<IPluginContext> AddPlugin;

        /// <summary>
        /// An event that fires when an plugin is removed.
        /// </summary>
        event EventHandler<IPluginContext> RemovePlugin;

        /// <summary>
        /// Returns all plugins.
        /// </summary>
        IEnumerable<IPluginContext> Plugins { get; }

        /// <summary>
        /// Returns a plugin context based on its id.
        /// </summary>
        /// <param name="pluginId">The id of the plugin.</param>
        /// <returns>The plugin context.</returns>
        IPluginContext GetPlugin(string pluginId);

        /// <summary>
        /// Returns a plugin context based on its id.
        /// </summary>
        /// <param name="pluginId">The type of the plugin.</param>
        /// <returns>The plugin context.</returns>
        IPluginContext GetPlugin(Type plugin);
    }
}
