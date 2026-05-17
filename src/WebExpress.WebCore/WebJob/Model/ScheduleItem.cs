using System;
using System.Threading;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebJob.Model
{
    /// <summary>
    /// Represents an appointment entry in the appointment execution directory.
    /// </summary>
    internal class ScheduleItem : IDisposable
    {
        /// <summary>
        /// Gets the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Gets the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Gets the context associated with the job.
        /// </summary>
        public IJobContext JobContext { get; internal set; }

        /// <summary>
        /// Gets the job class.
        /// </summary>
        public Type JobClass { get; internal set; }

        /// <summary>
        /// Gets the job instance.
        /// </summary>
        public IJob Instance { get; internal set; }

        /// <summary>
        /// Gets the cancel token or null if not already created.
        /// </summary>
        public CancellationTokenSource TokenSource { get; } = new CancellationTokenSource();

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The associated component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        /// <param name="pluginContext">The associated plugin context.</param>
        /// <param name="applicationContext">The corresponding application context.</param>
        /// <param name="jobContext">The job context.</param>
        /// <param name="jobClass">The job class.</param>
        public ScheduleItem(IComponentHub componentHub, IHttpServerContext httpServerContext, IPluginContext pluginContext, IApplicationContext applicationContext, IJobContext jobContext, Type jobClass)
        {
            PluginContext = pluginContext;
            ApplicationContext = applicationContext;
            JobContext = jobContext;
            JobClass = jobClass;

            Instance = ComponentActivator.CreateInstance<IJob, IJobContext>
            (
                jobClass,
                jobContext,
                httpServerContext,
                componentHub,
                pluginContext,
                applicationContext
            );

            return;
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Convert the resource element to a string.
        /// </summary>
        /// <returns>The resource element in its string representation.</returns>
        public override string ToString()
        {
            return "Job ${Id}";
        }
    }
}
