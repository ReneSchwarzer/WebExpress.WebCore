using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebModule;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy module for testing purposes.
    /// </summary>
    [Application<TestApplication>()]
    [Name("module.name")]
    [Description("module.description")]
    [Icon("/assets/img/Logo.png")]
    [AssetPath("/")]
    [ContextPath("/")]
    public sealed class TestModule : IModule
    {
        /// <summary>
        /// Initialization of the module.
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        public void Initialization(IModuleContext moduleContext)
        {
            throw new NotImplementedException();
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
