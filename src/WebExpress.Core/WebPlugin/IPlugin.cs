using System;

namespace WebExpress.Core.WebPlugin
{
    /// <summary>
    /// This interface represents a plugin.
    /// </summary>
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// Initialization of the plugin.
        /// </summary>
        /// <param name="context">The context.</param>
        void Initialization(IPluginContext context);

        /// <summary>
        /// Called when the plugin starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
