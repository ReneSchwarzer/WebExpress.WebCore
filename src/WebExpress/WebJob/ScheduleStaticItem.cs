using System;
using System.Collections.Generic;
using System.Reflection;
using WebExpress.WebModule;
using WebExpress.WebPlugin;

namespace WebExpress.WebJob
{
    /// <summary>
    /// Represents an appointment entry in the appointment execution directory
    /// </summary>
    internal class ScheduleStaticItem
    {
        /// <summary>
        /// The assembly that contains the module.
        /// </summary>
        public Assembly Assembly { get; internal set; }

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
        /// Returns the cron object.
        /// </summary>
        public Cron Cron { get; internal set; }

        /// <summary>
        /// Returns the log to write status messages to the console and to a log file.
        /// </summary>
        public Log Log { get; internal set; }

        /// <summary>
        /// Returns the job class.
        /// </summary>
        public Type Type { get; internal set; }

        /// <summary>
        /// Returns or sets the module id.
        /// </summary>
        public string moduleId { get; set; }

        /// <summary>
        /// Returns the directory where the job instances are listed.
        /// </summary>
        public IDictionary<IModuleContext, ScheduleStaticItemValue> Dictionary { get; }
            = new Dictionary<IModuleContext, ScheduleStaticItemValue>();

        /// <summary>
        /// An event that fires when an job is added.
        /// </summary>
        public event EventHandler<IJobContext> AddJob;

        /// <summary>
        /// An event that fires when an job is removed.
        /// </summary>
        public event EventHandler<IJobContext> RemoveJob;

        /// <summary>
        /// Adds an module assignment
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        public void AddModule(IModuleContext moduleContext)
        {
            // only if no instance has been created yet
            if (Dictionary.ContainsKey(moduleContext))
            {
                return;
            }

            // Only for the right module
            if (!moduleContext.ModuleId.Equals(moduleId, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // create context
            var jobContext = new JobContext(moduleContext)
            {
                JobId = JobId,
                PluginContext = PluginContext,
                Cron = Cron
            };

            var jobInstance = Activator.CreateInstance(Type) as IJob;
            jobInstance.Initialization(jobContext);

            Dictionary.Add
            (
                moduleContext,
                new ScheduleStaticItemValue()
                {
                    JobContext = jobContext,
                    Instance = jobInstance
                }
            );

            OnAddJob(jobContext);
        }

        /// <summary>
        /// Remove an module assignment
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        public void DetachModule(IModuleContext moduleContext)
        {
            // not an assignment has been created yet
            if (!Dictionary.ContainsKey(moduleContext))
            {
                return;
            }

            foreach (var scheduleItemValue in Dictionary.Values)
            {
                OnRemoveResource(scheduleItemValue.JobContext);
            }

            Dictionary.Remove(moduleContext);
        }

        /// <summary>
        /// Raises the AddJob event.
        /// </summary>
        /// <param name="resourceContext">The job context.</param>
        private void OnAddJob(IJobContext jobContext)
        {
            AddJob?.Invoke(this, jobContext);
        }

        /// <summary>
        /// Raises the RemoveJob event.
        /// </summary>
        /// <param name="resourceContext">The job context.</param>
        private void OnRemoveResource(IJobContext jobContext)
        {
            RemoveJob?.Invoke(this, jobContext);
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var d in AddJob.GetInvocationList())
            {
                AddJob -= (EventHandler<IJobContext>)d;
            }

            foreach (var d in RemoveJob.GetInvocationList())
            {
                RemoveJob -= (EventHandler<IJobContext>)d;
            }

            foreach (var scheduleItemValue in Dictionary.Values)
            {
                scheduleItemValue.TokenSource.Cancel();
            }
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
