using System;

namespace WebExpress.WebCore.WebPlugin
{
    /// <summary>
    /// This interface represents a plugin.
    /// </summary>
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// Initialization of the plugin. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        void Initialization(IPluginContext pluginContext);

        /// <summary>
        /// Called when the plugin starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
