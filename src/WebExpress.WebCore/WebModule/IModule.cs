using System;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebModule
{
    /// <summary>
    /// Interface of a module
    /// </summary>
    public interface IModule : IComponent, IDisposable
    {
        /// <summary>
        /// Called when the module starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
