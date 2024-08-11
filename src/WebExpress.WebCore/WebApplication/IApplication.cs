using System;

namespace WebExpress.WebCore.WebApplication
{
    /// <summary>
    /// This interface represents an application.
    /// </summary>
    public interface IApplication : IDisposable
    {
        /// <summary>
        /// Initialization of the application. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        void Initialization(IApplicationContext applicationContext);

        /// <summary>
        /// Called when the application starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
