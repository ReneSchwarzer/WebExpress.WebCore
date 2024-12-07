using System;
using System.Collections.Generic;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebTask.Model;

namespace WebExpress.WebCore.WebTask
{
    /// <summary>
    /// Management of ad-hoc tasks.
    /// </summary>
    public class TaskManager : ITaskManager, ISystemComponent
    {
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly TaskDictionary _dictionary = [];

        /// <summary>
        /// Returns the collection of tasks.
        /// </summary>
        public IEnumerable<ITask> Tasks => _dictionary.Values;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private TaskManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;
            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress.webcore:applicationmanager.initialization")
            );
        }

        /// <summary>
        /// Checks if a task has already been created.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <returns>True if this task already exists, false otherwise.</returns>
        public bool ContainsTask(string id)
        {
            return _dictionary.ContainsKey(id?.ToLower());
        }

        /// <summary>
        /// Returns an existing task.
        /// </summary>
        /// <param name="id">The id of the task.</param>
        /// <returns>The task or null.</returns>
        public ITask GetTask(string id)
        {
            if (_dictionary.ContainsKey(id?.ToLower()))
            {
                return _dictionary[id?.ToLower()];
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

            if (_dictionary.TryGetValue(id, out var value))
            {
                return value;
            }

            var task = ComponentActivator.CreateInstance<Task>(_httpServerContext, _componentHub, [id, args]);
            _dictionary.Add(key, task);

            return task;
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
        public ITask CreateTask<T>(string id, EventHandler<TaskEventArgs> handler, params object[] args) where T : Task
        {
            var key = id?.ToLower();

            if (_dictionary.TryGetValue(id, out var value))
            {
                return value;
            }

            var task = ComponentActivator.CreateInstance<T>(_httpServerContext, _componentHub, [id, args]);
            _dictionary.Add(key, task);

            task.Process += handler;

            return task;
        }

        /// <summary>
        /// Removes a task.
        /// </summary>
        /// <param name="task">The task.</param>
        public void RemoveTask(ITask task)
        {
            var key = task?.Id.ToLower();

            _dictionary.Remove(key);
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
