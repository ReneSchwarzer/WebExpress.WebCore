using WebExpress.WebCore.Internationalization;

namespace WebExpress.WebCore.WebJob
{
    /// <summary>
    /// A task that can be performed cyclically.
    /// </summary>
    public interface IJob : II18N
    {
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The context in which the job is executed.</param>
        public void Initialization(IJobContext context);

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        public void Process();

    }
}
