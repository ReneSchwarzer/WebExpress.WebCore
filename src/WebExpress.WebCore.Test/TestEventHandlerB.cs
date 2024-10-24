using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebEvent;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy event handler for testing purposes.
    /// </summary>
    [Application<TestApplicationB>()]
    [Event<TestEventB>]
    public sealed class TestEventHandlerB : IEventHandler<TestEventArgument>
    {
        /// <summary>
        /// Initialization of the event.
        /// </summary>
        /// <param name="eventHandlerContext">The event handler context, for testing the injection.</param>
        /// <param name="applicationManager">The application manager, for testing the injection.</param>
        private TestEventHandlerB(IEventHandlerContext eventHandlerContext, IApplicationManager applicationManager)
        {
            // test the injection
            if (eventHandlerContext == null)
            {
                throw new ArgumentNullException(nameof(eventHandlerContext), "Parameter cannot be null or empty.");
            }

            // test the injection
            if (applicationManager == null)
            {
                throw new ArgumentNullException(nameof(applicationManager), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Process the event.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="eventArgument">The argument for the event.</param>
        public void Process(object sender, TestEventArgument eventArgument)
        {
            // test the parameter
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender), "Parameter cannot be null or empty.");
            }

            // test the parameter
            if (eventArgument == null)
            {
                throw new ArgumentNullException(nameof(eventArgument), "Parameter cannot be null or empty.");
            }

            // to check in the test whether the handler has been executed correctly
            eventArgument.TestProperty = true;
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
