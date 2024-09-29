using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebModule;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy module for testing purposes.
    /// </summary>
    [Application<TestApplicationC>()]
    [ContextPath("/mcc")]
    [AssetPath("/mac")]
    [DataPath("/mdc")]
    public sealed class TestModuleC1 : IModule
    {
        /// <summary>
        /// Initialization of the module.
        /// </summary>
        /// <param name="moduleContext">The module context, for testing the injection.</param>
        /// <param name="applicationContext">The application manager, for testing the injection.</param>
        private TestModuleC1(IModuleContext moduleContext, ApplicationManager applicationManager)
        {
            // test the injection
            if (moduleContext == null)
            {
                throw new ArgumentNullException(nameof(moduleContext), "Parameter cannot be null or empty.");
            }

            // test the injection
            if (applicationManager == null)
            {
                throw new ArgumentNullException(nameof(applicationManager), "Parameter cannot be null or empty.");
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
