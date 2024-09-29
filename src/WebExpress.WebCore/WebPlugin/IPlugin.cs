using System;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebPlugin
{
    /// <summary>
    /// This interface represents a plugin.
    /// </summary>
    public interface IPlugin : IComponent, IDisposable
    {
        /// <summary>
        /// Called when the plugin starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
