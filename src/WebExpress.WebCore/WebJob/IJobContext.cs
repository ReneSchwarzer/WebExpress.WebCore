using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebJob
{
    /// <summary>
    /// Represents the context of a job.
    /// </summary>
    public interface IJobContext : IContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

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
