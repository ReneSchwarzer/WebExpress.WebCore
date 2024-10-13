using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebEvent
{
    /// <summary>
    /// Represents the context of an event.
    /// </summary>
    public interface IEventHandlerContext : IContext
    {
        /// <summary>
        /// Returns the event id.
        /// </summary>
        string EventId { get; }

        /// <summary>
        /// Returns the event handler id.
        /// </summary>
        string EventHandlerId { get; }

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }
    }
}
