using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebEvent
{
    /// <summary>
    /// Represents an event handler.
    /// </summary>
    public interface IEventHandler : IComponent
    {
        /// <summary>
        /// Process the event.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="eventArgument">The argument for the event.</param>
        void Process(object sender, IEventArgument eventArgument);
    }

    /// <summary>
    /// Represents an event handler.
    /// </summary>
    public interface IEventHandler<T> : IComponent where T : class, IEventArgument
    {
        /// <summary>
        /// Process the event.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="eventArgument">The argument for the event.</param>
        void Process(object sender, T eventArgument);
    }
}
