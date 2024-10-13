using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebEvent
{
    /// <summary>
    /// Represents the context of an event.
    /// </summary>
    public class EventHandlerContext : IEventHandlerContext
    {
        /// <summary>
        /// Returns the event id.
        /// </summary>
        public string EventId { get; internal set; }

        /// <summary>
        /// Returns the event handler id.
        /// </summary>
        public string EventHandlerId { get; internal set; }

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }
    }
}
