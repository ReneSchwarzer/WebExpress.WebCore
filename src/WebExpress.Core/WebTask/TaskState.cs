namespace WebExpress.Core.WebTask
{
    public enum TaskState
    {
        /// <summary>
        /// The task has been created and is waiting to be executed.
        /// </summary>
        Created,
        /// <summary>
        /// The task is in the process of being executed.
        /// </summary>
        Run,
        /// <summary>
        /// The task was aborted.
        /// </summary>
        Canceled,
        /// <summary>
        /// The task has ended.
        /// </summary>
        Finish
    }
}
