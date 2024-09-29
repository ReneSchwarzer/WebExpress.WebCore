using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebModule;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy module for testing purposes.
    /// </summary>
    [Application<TestApplicationA>()]
    [Name("module.namea2")]
    [Description("module.descriptiona2")]
    [ContextPath(null)]
    [Icon("/assets/img/Logo.png")]
    public sealed class TestModuleA2 : IModule
    {
        /// <summary>
        /// Initialization of the module.
        /// </summary>
        /// <param name="moduleContext">The module context, for testing the injection.</param>
        private TestModuleA2(IModuleContext moduleContext)
        {
            // test the injection
            if (moduleContext == null)
            {
                throw new ArgumentNullException(nameof(moduleContext), "Parameter cannot be null or empty.");
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
