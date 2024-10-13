using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEvent.Model;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebEvent
{
    /// <summary>
    /// The event manager.
    /// </summary>
    public sealed class EventManager : IEventManager, IComponentManagerPlugin, ISystemComponent
    {
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly EventDictionary _dictionary = [];

        /// <summary>
        /// An event that fires when an event handler is added.
        /// </summary>
        public event EventHandler<IEventHandlerContext> AddEventHandler;

        /// <summary>
        /// An event that fires when an event handler is removed.
        /// </summary>
        public event EventHandler<IEventHandlerContext> RemoveEventHandler;

        /// <summary>
        /// Returns the collection of events.
        /// </summary>
        public IEnumerable<IEventHandlerContext> EventHandlers => _dictionary.Values
            .SelectMany(x => x.Values)
            .SelectMany(x => x.Values)
            .SelectMany(x => x)
            .Select(x => x.EventHandlerContext);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        internal EventManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;

            _componentHub.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            _componentHub.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate
                (
                    "webexpress:eventmanager.initialization"
                )
            );
        }

        /// <summary>
        /// Returns the event handler contexts.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of event handler contexts.</returns>
        public IEnumerable<IEventHandlerContext> GetEventHandlers<T>(IApplicationContext applicationContext) where T : IEvent
        {
            return _dictionary.GetEventHandlerItems<T>(applicationContext)
                .Select(x => x.EventHandlerContext);
        }

        /// <summary>
        /// Returns the event handler contexts.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="eventType">The type of event.</param>
        /// <returns>An IEnumerable of event handler contexts.</returns>
        public IEnumerable<IEventHandlerContext> GetEventHandlers(IApplicationContext applicationContext, Type eventType)
        {
            return _dictionary.GetEventHandlerItems(applicationContext, eventType)
                .Select(x => x.EventHandlerContext);
        }

        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="sender">The sender object.</param>
        /// <param name="argument">The event argument.</param>
        public void RaiseEvent<T>(IApplicationContext applicationContext, object sender, IEventArgument argument) where T : IEvent
        {
            var eventHandlers = _dictionary.GetEventHandlerItems<T>(applicationContext);

            foreach (var eventHandler in eventHandlers)
            {
                eventHandler?.Process(sender, argument);
            }
        }

        /// <summary>
        /// Discovers and registers event handlers from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose event handlers are to be registered.</param>
        public void Register(IPluginContext pluginContext)
        {
            var assembly = pluginContext?.Assembly;

            foreach (var eventHandlerType in assembly.GetTypes().Where
                (
                    x => x.IsClass == true &&
                    x.IsSealed &&
                    x.IsPublic &&
                    (
                        x.GetInterface(typeof(IEventHandler).Name) != null ||
                        x.GetInterface(typeof(IEventHandler<>).Name) != null
                    )
                ))
            {
                var id = eventHandlerType.FullName?.ToLower();
                var applicationIds = new List<string>();
                var eventType = default(Type);

                foreach (var customAttribute in eventHandlerType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IEventAttribute))))
                {
                    if (customAttribute.AttributeType.Name == typeof(ApplicationAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ApplicationAttribute<>).Namespace)
                    {
                        applicationIds.Add(customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower());
                    }
                    else if (customAttribute.AttributeType.Name == typeof(EventAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(EventAttribute<>).Namespace)
                    {
                        eventType = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                    }
                }

                if (!applicationIds.Any())
                {
                    // no application specified
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate("webexpress:eventmanager.applicationless", id)
                    );

                    break;
                }

                if (applicationIds.Count() > 1)
                {
                    // too many specified applications
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate("webexpress:eventmanager.applicationrich", id)
                    );
                }

                // assign the module to existing applications.
                var applicationContext = _componentHub.ApplicationManager.GetApplication(applicationIds.FirstOrDefault());

                if (eventType != default)
                {
                    var eventHandlerContext = new EventHandlerContext()
                    {
                        EventId = eventType.FullName.ToLower(),
                        EventHandlerId = id,
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext
                    };

                    if (_dictionary.AddEventItem
                    (
                        pluginContext,
                        applicationContext,
                        new EventItem(_componentHub, pluginContext, applicationContext, eventHandlerContext, eventHandlerType, eventType)
                    ))
                    {
                        OnAddEventHandler(eventHandlerContext);

                        _httpServerContext.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress:eventmanager.register",
                                id,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                    else
                    {
                        _httpServerContext.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress:eventmanager.duplicate",
                                id,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                }
                else
                {
                    _httpServerContext.Log.Debug
                    (
                        I18N.Translate
                        (
                            "webexpress:eventmanager.eventless",
                            id
                        )
                    );
                }
            }
        }

        /// <summary>
        /// Discovers and registers entries from the specified plugin.
        /// </summary>
        /// <param name="pluginContexts">A list with plugin contexts that contain the jobs.</param>
        public void Register(IEnumerable<IPluginContext> pluginContexts)
        {
            foreach (var pluginContext in pluginContexts)
            {
                Register(pluginContext);
            }
        }

        /// <summary>
        /// Removes all jobs associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the event to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            // the plugin has not been registered in the manager
            if (_dictionary.TryGetValue(pluginContext, out var value))
            {
                foreach (var eventHandlerItem in value
                    .SelectMany(x => x.Value)
                    .SelectMany(x => x.Value))
                {
                    eventHandlerItem.Dispose();
                }

                _dictionary.Remove(pluginContext);
            }
        }

        /// <summary>
        /// Raises the AddEventHandler event.
        /// </summary>
        /// <param name="eventHandlerContext">The event handler context.</param>
        private void OnAddEventHandler(IEventHandlerContext eventHandlerContext)
        {
            AddEventHandler?.Invoke(this, eventHandlerContext);
        }

        /// <summary>
        /// Raises the RemoveEventHandler event.
        /// </summary>
        /// <param name="eventHandlerContext">The event handler context.</param>
        private void OnRemoveEventHandler(IEventHandlerContext eventHandlerContext)
        {
            RemoveEventHandler?.Invoke(this, eventHandlerContext);
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the event.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
            //foreach (var scheduleItem in GetScheduleItems(pluginContext))
            //{
            //    output.Add
            //    (
            //        string.Empty.PadRight(deep) +
            //        I18N.Translate
            //        (
            //            "webexpress:eventmanager.job",
            //            scheduleItem.JobId,
            //            scheduleItem.ModuleContext
            //        )
            //    );
            //}
        }
    }
}
