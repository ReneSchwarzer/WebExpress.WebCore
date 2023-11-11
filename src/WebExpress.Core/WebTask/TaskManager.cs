using System;
using System.Collections.Generic;
using WebExpress.Core.Internationalization;
using WebExpress.Core.WebComponent;
using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebTask
{
    /// <summary>
    /// Management of ad-hoc tasks.
    /// </summary>
    public class TaskManager : IComponent, ISystemComponent
    {
        /// <summary>
        /// Returns or sets the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns the directory in which the active jobs are listed.
        /// </summary>
        private TaskDictionary Dictionary { get; } = new TaskDictionary();

        /// <summary>
        /// Constructor
        /// </summary>
        internal TaskManager()
        {
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The reference to the context of the host.</param>
        public void Initialization(IHttpServerContext context)
        {
            HttpServerContext = context;

            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N("webexpress:applicationmanager.initialization")
            );
        }

        /// <summary>
        /// Checks if a task has already been created.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <returns>True if this task already exists, false otherwise.</returns>
        public bool ContainsTask(string id)
        {
            return Dictionary.ContainsKey(id?.ToLower());
        }

        /// <summary>
        /// Returns an existing task.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <returns>The task or null.</returns>
        public ITask GetTask(string id)
        {
            if (Dictionary.ContainsKey(id?.ToLower()))
            {
                return Dictionary[id?.ToLower()];
            }

            return null;
        }

        /// <summary>
        /// Creates a new task or returns an existing task.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <param name="args">The event argument.</param>
        /// <returns>The task or null.</returns>
        public ITask CreateTask(string id, params object[] args)
        {
            var key = id?.ToLower();

            if (!Dictionary.ContainsKey(id))
            {
                var task = new Task() { Id = id, State = TaskState.Created, Arguments = args };
                Dictionary.Add(key, task);

                task.Initialization();

                return task;
            }

            return Dictionary[id];
        }

        /// <summary>
        /// Creates a new task or returns an existing task.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <param name="handler">The event handler.</param>
        /// <param name="args">The event argument.</param>
        /// <returns>The task or null.</returns>
        public ITask CreateTask(string id, EventHandler<TaskEventArgs> handler, params object[] args)
        {
            return CreateTask<Task>(id, handler, args);
        }

        /// <summary>
        /// Creates a new task or returns an existing task.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <param name="handler">The event handler.</param>
        /// <param name="args">The event argument.</param>
        /// <returns>The task or null.</returns>
        public ITask CreateTask<T>(string id, EventHandler<TaskEventArgs> handler, params object[] args) where T : Task, new()
        {
            var key = id?.ToLower();

            if (!Dictionary.ContainsKey(id))
            {
                var task = new Task() { Id = id, State = TaskState.Created, Arguments = args };
                Dictionary.Add(key, task);

                task.Initialization();

                task.Process += handler;

                return task;
            }

            return Dictionary[id];
        }

        /// <summary>
        /// Removes a task.
        /// </summary>
        /// <param name="task">The task.</param>
        public void RemoveTask(ITask task)
        {
            var key = task?.Id.ToLower();

            if (Dictionary.ContainsKey(key))
            {
                Dictionary.Remove(key);
            }
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
        }
    }
}
