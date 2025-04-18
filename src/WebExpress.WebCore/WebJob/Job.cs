namespace WebExpress.WebCore.WebJob
{
    /// <summary>
    /// A task that can be performed cyclically.
    /// </summary>
    public class Job : IJob
    {
        /// <summary>
        /// Processing of the resource.
        /// </summary>
        public virtual void Process()
        {

        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
