using System;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebTask
{
    /// <summary>
    /// Represents a task that can be executed, monitored, and controlled.
    /// </summary>
    public interface ITask : IComponent
    {
        /// <summary>
        /// Event is triggered when the task is executed.
        /// </summary>
        event EventHandler<TaskEventArgs> Process;

        /// <summary>
        /// Event is triggered when the task ends.
        /// </summary>
        event EventHandler<TaskEventArgs> Finish;

        /// <summary>
        /// Gets the id of the task.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the state in which the task is located.
        /// </summary>
        TaskState State { get; }

        /// <summary>
        /// Gets or sets the progress of the task. The value range is from 0 to 100.
        /// </summary>
        int Progress { get; set; }

        /// <summary>
        /// Gets or sets a message that provides information about the processing.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Starts the execution concurrently.
        /// </summary>
        void Run();

        /// <summary>
        /// Abandonment of an existing processing.
        /// </summary>
        void Cancel();
    }
}
