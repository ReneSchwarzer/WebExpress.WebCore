using System;

namespace WebExpress.WebCore.WebApplication
{
    /// <summary>
    /// This interface represents an application.
    /// </summary>
    public interface IApplication : IDisposable
    {
        /// <summary>
        /// Returns the context of the application.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Initialization of the application .
        /// </summary>
        /// <param name="applicationContext">The context.</param>
        void Initialization(IApplicationContext applicationContext);

        /// <summary>
        /// Called when the application starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
