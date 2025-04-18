using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebJob
{
    /// <summary>
    /// Processing of cyclic jobs.
    /// </summary>
    public interface IJobManager : IComponentManager
    {
        /// <summary>
        /// Returns all jobs contextes.
        /// </summary>
        IEnumerable<IJobContext> Jobs { get; }

        /// <summary>
        /// Returns a JobContext instance associated with an application.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="jobType">The type of the job.</param>
        /// <returns>A JobContext instance.</returns>
        IJobContext GetJob(IApplicationContext applicationContext, Type jobType);
    }
}
