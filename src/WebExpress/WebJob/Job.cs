
using System.Globalization;

namespace WebExpress.WebJob
{
    /// <summary>
    /// A task that can be performed cyclically.
    /// </summary>
    public class Job : IJob
    {
        /// <summary>
        /// Returns or sets the culture.
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Returns the context.
        /// </summary>
        public IJobContext Context { get; private set; }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The context in which the job is executed.</param>
        public virtual void Initialization(IJobContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        public virtual void Process()
        {

        }
    }
}
