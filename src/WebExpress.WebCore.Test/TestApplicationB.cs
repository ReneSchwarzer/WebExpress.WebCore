using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy application for testing purposes.
    /// </summary>
    [Name("TestApplicationB")]
    [Description("application.description")]
    [Icon("/assets/img/Logo.png")]
    [ContextPath("/acb")]
    [AssetPath("/aab")]
    [DataPath("/adb")]
    [Dependency("webexpress.webui")]
    public sealed class TestApplicationB : IApplication
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private TestApplicationB()
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
