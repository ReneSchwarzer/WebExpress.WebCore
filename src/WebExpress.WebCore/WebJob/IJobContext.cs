using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebJob
{
    public interface IJobContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the corresponding module context.
        /// </summary>
        IModuleContext ModuleContext { get; }

        /// <summary>
        /// Returns the job id. 
        /// </summary>
        string JobId { get; }

        /// <summary>
        /// Returns the cron-object.
        /// </summary>
        Cron Cron { get; }
    }
}
