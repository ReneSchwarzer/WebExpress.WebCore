using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebJob
{
    /// <summary>
    /// Represents the job context.
    /// </summary>
    public class JobContext : IJobContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns the job id. 
        /// </summary>
        public string JobId { get; internal set; }

        /// <summary>
        /// Returns the cron-object.
        /// </summary>
        public Cron Cron { get; internal set; }
    }
}
