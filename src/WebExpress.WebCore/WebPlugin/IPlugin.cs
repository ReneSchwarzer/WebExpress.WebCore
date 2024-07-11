using System;

namespace WebExpress.WebCore.WebPlugin
{
    /// <summary>
    /// This interface represents a plugin.
    /// </summary>
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// Returns the context of the plugin.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Initialization of the plugin.
        /// </summary>
        /// <param name="pluginContext">The context.</param>
        void Initialization(IPluginContext pluginContext);

        /// <summary>
        /// Called when the plugin starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
