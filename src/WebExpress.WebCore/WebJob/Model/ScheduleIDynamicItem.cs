using System.Threading;

namespace WebExpress.WebCore.WebJob.Model
{
    /// <summary>
    /// Represents an job entry in the dynamic job execution list.
    /// </summary>
    internal class ScheduleDynamicItem
    {
        /// <summary>
        /// The context associated with the job.
        /// </summary>
        public IJobContext JobContext { get; set; }

        /// <summary>
        /// Returns the job instance.
        /// </summary>
        public IJob Instance { get; internal set; }

        /// <summary>
        /// Returns the cancel token or null if not already created.
        /// </summary>
        public CancellationTokenSource TokenSource { get; } = new CancellationTokenSource();
    }
}
