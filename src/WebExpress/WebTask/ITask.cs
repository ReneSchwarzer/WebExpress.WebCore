using System;

namespace WebExpress.WebTask
{
    public interface ITask
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
        /// The id of the task.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Returns the state in which the task is located.
        /// </summary>
        TaskState State { get; }

        /// <summary>
        /// Returns the progress of the task. The value range is from 0 to 100.
        /// </summary>
        int Progress { get; set; }

        /// <summary>
        /// Returns or sets a message that provides information about the processing.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Initialization
        /// </summary>
        void Initialization();

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
