using WebExpress.Core.WebModule;
using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebJob
{
    public class JobContext : IJobContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the corresponding module context.
        /// </summary>
        public IModuleContext ModuleContext { get; internal set; }

        /// <summary>
        /// Returns the job id. 
        /// </summary>
        public string JobId { get; internal set; }

        /// <summary>
        /// Returns the cron-object.
        /// </summary>
        public Cron Cron { get; internal set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        internal JobContext(IModuleContext moduleContext)
        {
            PluginContext = moduleContext?.PluginContext;
            ModuleContext = moduleContext;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        internal JobContext(IPluginContext pluginContext)
        {
            PluginContext = pluginContext;
        }
    }
}
