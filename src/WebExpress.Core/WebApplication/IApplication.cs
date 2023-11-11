using System;

namespace WebExpress.Core.WebApplication
{
    /// <summary>
    /// This interface represents an application.
    /// </summary>
    public interface IApplication : IDisposable
    {
        /// <summary>
        /// Initialization of the application .
        /// </summary>
        /// <param name="context">The context.</param>
        void Initialization(IApplicationContext context);

        /// <summary>
        /// Called when the application starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
