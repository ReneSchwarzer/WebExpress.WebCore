using System;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebApplication
{
    /// <summary>
    /// This interface represents an application.
    /// </summary>
    public interface IApplication : IComponent, IDisposable
    {
        /// <summary>
        /// Called when the application starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
