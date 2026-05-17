using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebEvent
{
    /// <summary>
    /// Represents the interface for managing web events.
    /// </summary>
    public interface IEventManager : IComponentManager
    {
        /// <summary>
        /// Gets the collection of events.
        /// </summary>
        IEnumerable<IEventHandlerContext> EventHandlers { get; }

        /// <summary>
        /// Returns the event handler contexts.
        /// </summary>
        /// <typeparam name="TEvent">The type of event.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of event handler contexts.</returns>
        IEnumerable<IEventHandlerContext> GetEventHandlers<TEvent>(IApplicationContext applicationContext)
            where TEvent : IEvent;

        /// <summary>
        /// Returns the event handler contexts.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="eventType">The type of event.</param>
        /// <returns>An IEnumerable of event handler contexts.</returns>
        IEnumerable<IEventHandlerContext> GetEventHandlers(IApplicationContext applicationContext, Type eventType);

        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of event.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="sender">The sender object.</param>
        /// <param name="argument">The event argument.</param>
        void RaiseEvent<TEvent>(IApplicationContext applicationContext, object sender, IEventArgument argument)
            where TEvent : IEvent;
    }
}
