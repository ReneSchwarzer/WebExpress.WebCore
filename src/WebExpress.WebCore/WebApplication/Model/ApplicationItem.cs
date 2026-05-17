using System;
using System.Threading;

namespace WebExpress.WebCore.WebApplication.Model
{
    /// <summary>
    /// Represents an application entry in the application directory.
    /// </summary>
    internal class ApplicationItem
    {
        /// <summary>
        /// Gets the context associated with the application.
        /// </summary>
        public IApplicationContext ApplicationContext { get; set; }

        /// <summary>
        /// Gets the application class.
        /// </summary>
        public Type ApplicationClass { get; internal set; }

        /// <summary>
        /// Gets the application.
        /// </summary>
        public IApplication Application { get; set; }

        /// <summary>
        /// Gets the thread termination token.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
    }
}
