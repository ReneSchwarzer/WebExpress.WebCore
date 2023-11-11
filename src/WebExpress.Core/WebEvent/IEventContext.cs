using WebExpress.Core.WebModule;
using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebEvent
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
