using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy application for testing purposes.
    /// </summary>
    [Name("TestApplicationA")]
    [Description("application.description")]
    [Icon("/assets/img/Logo.png")]
    [ContextPath("/appa")]
    [AssetPath("/asseta")]
    [DataPath("/dataa")]
    [Dependency("webexpress.webui")]
    public sealed class TestApplicationA : IApplication
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="applicationContext">The application context, for testing the injection.</param>
        private TestApplicationA(IApplicationContext applicationContext)
        {
            // test the injection
            if (applicationContext == null)
            {
                throw new ArgumentNullException(nameof(applicationContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Initialization of the application.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        public void Initialization(IApplicationContext applicationContext)
        {
        }

        /// <summary>
        /// Called when the plugin starts working. The call is concurrent.
        /// </summary>
        public void Run()
        {
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
