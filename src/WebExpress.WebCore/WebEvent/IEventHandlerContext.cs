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
        /// Gets the event id.
        /// </summary>
        IComponentId EventId { get; }

        /// <summary>
        /// Gets the event handler id.
        /// </summary>
        string EventHandlerId { get; }

        /// <summary>
        /// Gets the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Gets the corresponding application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }
    }
}
