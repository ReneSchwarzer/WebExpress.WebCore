using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebModule;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy module for testing purposes.
    /// </summary>
    [Application<TestApplicationA>()]
    [Name("module.namea1")]
    [Description("module.descriptiona1")]
    [Icon("/assets/img/Logo.png")]
    [ContextPath("/mca")]
    [AssetPath("/maa")]
    [DataPath("/mda")]
    public sealed class TestModuleA1 : IModule
    {
        /// <summary>
        /// Initialization of the module.
        /// </summary>
        /// <param name="moduleContext">The module context, for testing the injection.</param>
        private TestModuleA1(IModuleContext moduleContext)
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
