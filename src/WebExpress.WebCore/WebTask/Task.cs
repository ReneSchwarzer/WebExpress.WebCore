using System;
using System.Collections.Generic;
using System.Threading;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebTask
{
    public class Task : ITask
    {
        /// <summary>
        /// Internal management of progress.
        /// </summary>
        private int _Progress { get; set; }

        /// <summary>
        /// Event is triggered when the task is executed.
        /// </summary>
        public event EventHandler<TaskEventArgs> Process;

        /// <summary>
        /// Event is triggered when the task is terminated.
        /// </summary>
        public event EventHandler<TaskEventArgs> Finish;

        /// <summary>
        /// The id of the task.
        /// </summary>
        public string Id { get; internal set; }

        /// <summary>
        /// Returns the state in which the task is located.
        /// </summary>
        public TaskState State { get; internal set; }

        /// <summary>
        /// The arguments.
        /// </summary>
        public ICollection<object> Arguments { get; internal set; }

        /// <summary>
        /// Thread termination of the task.
        /// </summary>
        private CancellationTokenSource TokenSource { get; } = new CancellationTokenSource();

        /// <summary>
        /// Retiurns the progress of the task. The value range is from 0 to 100.
        /// </summary>
        public int Progress
        {
            get => _Progress;
            set => _Progress = Math.Min(value, 100);
        }

        /// <summary>
        /// Returns or sets a message that provides information about the processing.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initialization
        /// </summary>
        public virtual void Initialization()
        {
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        protected virtual void OnProcess()
        {
            Process?.Invoke(this, new TaskEventArgs());
        }

        /// <summary>
        /// Triggered when the task is complete.
        /// </summary>
        protected virtual void OnFinish()
        {
            Finish?.Invoke(this, new TaskEventArgs());
        }

        /// <summary>
        /// Starts the execution concurrently.
        /// </summary>
        public void Run()
        {
            System.Threading.Tasks.Task.Factory.StartNew((Action)(() =>
            {
                State = TaskState.Run;

                this.Progress = 0;

                OnProcess();

                this.Progress = 100;

                State = TaskState.Finish;

                OnFinish();

                ComponentManager.TaskManager.RemoveTask(this);

            }), TokenSource.Token);
        }

        /// <summary>
        /// Abandonment of an existing processing.
        /// </summary>
        public void Cancel()
        {
            TokenSource.Cancel();

            State = TaskState.Canceled;

            ComponentManager.TaskManager.RemoveTask(this);
        }
    }
}
