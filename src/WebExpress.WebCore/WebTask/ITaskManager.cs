using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebTask
{
    /// <summary>
    /// Management of ad-hoc tasks.
    /// </summary>
    public interface ITaskManager : IComponentManager
    {
        /// <summary>
        /// Event is triggered when a task's changes.
        /// </summary>
        event EventHandler<TaskEventArgs> TaskChanged;

        /// <summary>
        /// Gets the collection of tasks.
        /// </summary>
        IEnumerable<ITask> Tasks { get; }

        /// <summary>
        /// Checks if a task has already been created.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <returns>True if this task already exists, false otherwise.</returns>
        bool ContainsTask(string id);

        /// <summary>
        /// Returns an existing task.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <returns>The task or null.</returns>
        ITask GetTask(string id);

        /// <summary>
        /// Creates a new task or returns an existing task.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <param name="args">The event argument.</param>
        /// <returns>The task or null.</returns>
        ITask CreateTask(string id, params object[] args);

        /// <summary>
        /// Creates a new task or returns an existing task.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <param name="handler">The event handler.</param>
        /// <param name="args">The event argument.</param>
        /// <returns>The task or null.</returns>
        ITask CreateTask(string id, EventHandler<TaskEventArgs> handler, params object[] args);

        /// <summary>
        /// Creates a new task or returns an existing task.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <param name="handler">The event handler.</param>
        /// <param name="args">The event argument.</param>
        /// <returns>The task or null.</returns>
        ITask CreateTask<T>(string id, EventHandler<TaskEventArgs> handler, params object[] args) where T : Task;

        /// <summary>
        /// Removes a task.
        /// </summary>
        /// <param name="task">The task.</param>
        void RemoveTask(ITask task);
    }
}
