using System;
using WebExpress.WebCore.WebEvent;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifying a event.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EventAttribute<T> : Attribute, IEventAttribute where T : class, IEvent
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventAttribute()
        {

        }
    }
}
