using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// Represents a test task that implements the Task.
    /// </summary>
    public class TestTask : WebTask.Task
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub used for dependency injection.</param>
        /// <param name="id">The unique identifier for the task.</param>
        /// <param name="args">The arguments for the task.</param>
        public TestTask(IComponentHub componentHub, string id, params object[] args)
            : base(id, args)
        {
            // test the injection
            if (componentHub == null)
            {
                throw new ArgumentNullException(nameof(componentHub), "Parameter cannot be null or empty.");
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Parameter cannot be null or empty.");
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Parameter cannot be null or empty.");
            }
        }
    }
}
