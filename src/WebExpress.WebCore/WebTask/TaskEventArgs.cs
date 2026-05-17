using System;

namespace WebExpress.WebCore.WebTask
{
    /// <summary>
    /// Provides data for a task event.
    /// </summary>
    public class TaskEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the related task.
        /// </summary>
        public ITask Task { get; }

        /// <summary>
        /// Gets the current progress (if relevant).
        /// </summary>
        public int Progress { get; }

        /// <summary>
        /// Gets the current or new message (if relevant).
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="task">The related task.</param>
        /// <param name="progress">The current progress.</param>
        /// <param name="message">The current or new message.</param>
        public TaskEventArgs(ITask task, int progress = 0, string message = null)
        {
            Task = task;
            Progress = progress;
            Message = message;
        }
    }
}
