using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy application for testing purposes.
    /// </summary>
    [Id("TestApplicationC")]
    [Name("TestApplicationC")]
    [Description("application.description")]
    [Icon("/assets/img/Logo.png")]
    [Dependency("webexpress.webui")]
    public sealed class TestApplicationC : IApplication
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="applicationContext">The application context, for testing the injection.</param>
        /// <param name="pluginManager">The plugin manager, for testing the injection.</param>
        private TestApplicationC(IApplicationContext applicationContext, IPluginManager pluginManager)
        {
            // test the injection
            if (applicationContext == null)
            {
                throw new ArgumentNullException(nameof(applicationContext), "Parameter cannot be null or empty.");
            }

            // test the injection
            if (pluginManager == null)
            {
                throw new ArgumentNullException(nameof(pluginManager), "Parameter cannot be null or empty.");
            }
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
