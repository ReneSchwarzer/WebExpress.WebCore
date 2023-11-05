using WebExpress.WebModule;
using WebExpress.WebPlugin;

namespace WebExpress.WebEvent
{
    public interface IEventContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the corresponding module context.
        /// </summary>
        IModuleContext ModuleContext { get; }
    }
}
