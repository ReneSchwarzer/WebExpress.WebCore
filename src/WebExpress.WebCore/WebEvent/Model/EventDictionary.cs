using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebEvent.Model
{
    /// <summary>
    /// Represents a dictionary that provides a mapping from plugin contexts to dictionaries of application contexts and event handler.
    /// </summary>
    internal class EventDictionary : Dictionary<IPluginContext, Dictionary<IApplicationContext, Dictionary<Type, IList<EventItem>>>>
    {
        /// <summary>
        /// Adds a event item to the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="eventItem">The event item.</param>
        /// <returns>True if the event handler item was added successfully, false if an element with the same status code already exists.</returns>
        public bool AddEventItem(IPluginContext pluginContext, IApplicationContext applicationContext, EventItem eventItem)
        {
            var type = eventItem.EventClass;

            if (!typeof(IEvent).IsAssignableFrom(type))
            {
                return false;
            }

            if (!ContainsKey(pluginContext))
            {
                this[pluginContext] = [];
            }

            var appContextDict = this[pluginContext];

            if (!appContextDict.ContainsKey(applicationContext))
            {
                appContextDict[applicationContext] = [];
            }

            var eventDict = appContextDict[applicationContext];

            if (!eventDict.ContainsKey(type))
            {
                eventDict[type] = [];
            }

            var eventList = eventDict[type];

            if (eventList.Where(x => x.EventClass == type).Any())
            {
                return false; // item with the same event handler already exists
            }

            eventList.Add(eventItem);

            return true;
        }

        /// <summary>
        /// Removes a event from the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        public void RemoveEventHandler<T>(IPluginContext pluginContext, IApplicationContext applicationContext) where T : IEvent
        {
            var type = typeof(T);

            if (ContainsKey(pluginContext))
            {
                var appContextDict = this[pluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var eventDict = appContextDict[applicationContext];

                    if (eventDict.ContainsKey(type))
                    {
                        eventDict.Remove(type);

                        if (eventDict.Count == 0)
                        {
                            appContextDict.Remove(applicationContext);

                            if (appContextDict.Count == 0)
                            {
                                Remove(pluginContext);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the event handler from the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of event items</returns>
        public IEnumerable<EventItem> GetEventHandlerItems<T>(IApplicationContext applicationContext) where T : IEvent
        {
            return GetEventHandlerItems(applicationContext, typeof(T));
        }

        /// <summary>
        /// Returns the event handler from the dictionary.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <typeparam name="eventType">The type of event.</typeparam>
        /// <returns>An IEnumerable of event items</returns>
        public IEnumerable<EventItem> GetEventHandlerItems(IApplicationContext applicationContext, Type eventType)
        {
            if (!typeof(IEvent).IsAssignableFrom(eventType))
            {
                return [];
            }

            if (ContainsKey(applicationContext?.PluginContext))
            {
                var appContextDict = this[applicationContext?.PluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var eventDict = appContextDict[applicationContext];

                    if (eventDict.ContainsKey(eventType))
                    {
                        return eventDict[eventType];
                    }
                }
            }

            return [];
        }

        /// <summary>
        /// Returns all event handler contexts for a given plugin context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <returns>An IEnumerable of event handler contexts.</returns>
        public IEnumerable<IEventHandlerContext> GetEventHandlers(IPluginContext pluginContext)
        {
            return this.Where(entry => entry.Key == pluginContext)
                       .SelectMany(entry => entry.Value.Values)
                       .SelectMany(dict => dict.Values)
                       .SelectMany(x => x)
                       .Select(x => x.EventHandlerContext);
        }
    }
}
