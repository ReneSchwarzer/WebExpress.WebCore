using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy plugin for testing purposes.
    /// </summary>
    [Name("webexpress.webui.unittest")]
    [Description("plugin.description")]
    [Icon("/assets/img/Logo.png")]
    public sealed class TestPlugin : IPlugin
    {
        /// <summary>
        /// Initialization of the plugin.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        public void Initialization(IPluginContext pluginContext)
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
