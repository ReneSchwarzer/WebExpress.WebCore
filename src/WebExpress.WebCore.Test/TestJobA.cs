﻿using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebJob;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy job for testing purposes.
    /// </summary>
    [Job("50", "8", "31", "1-2", "Saturday")]
    public sealed class TestJobA : IJob
    {
        /// <summary>
        /// Initialization of the job.
        /// </summary>
        /// <param name="jobContext">The job context, for testing the injection.</param>
        private TestJobA(IJobContext jobContext)
        {
            // test the injection
            if (jobContext == null)
            {
                throw new ArgumentNullException(nameof(jobContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Called when the jobs starts working. The call is concurrent.
        /// </summary>
        public void Process()
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
