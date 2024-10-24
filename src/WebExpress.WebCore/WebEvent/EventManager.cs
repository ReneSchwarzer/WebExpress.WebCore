﻿using System;
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
    public sealed class EventManager : IEventManager, ISystemComponent
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

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;

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
        /// Discovers and binds jobs to an application.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin whose jobs are to be associated.</param>
        private void Register(IPluginContext pluginContext)
        {
            if (_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            Register(pluginContext, _componentHub.ApplicationManager.GetApplications(pluginContext));
        }

        /// <summary>
        /// Discovers and binds jobs to an application.
        /// </summary>
        /// <param name="applicationContext">The context of the application whose jobs are to be associated.</param>
        private void Register(IApplicationContext applicationContext)
        {
            foreach (var pluginContext in _componentHub.PluginManager.GetPlugins(applicationContext))
            {
                if (_dictionary.TryGetValue(pluginContext, out var appDict) && appDict.ContainsKey(applicationContext))
                {
                    continue;
                }

                Register(pluginContext, [applicationContext]);
            }
        }

        /// <summary>
        /// Registers resources for a given plugin and application context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context (optional).</param>
        private void Register(IPluginContext pluginContext, IEnumerable<IApplicationContext> applicationContexts)
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
                var eventType = default(Type);

                foreach (var customAttribute in eventHandlerType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IEventAttribute))))
                {
                    if (customAttribute.AttributeType.Name == typeof(EventAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(EventAttribute<>).Namespace)
                    {
                        eventType = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                    }
                }

                if (eventType == default)
                {
                    _httpServerContext.Log.Debug
                    (
                        I18N.Translate
                        (
                            "webexpress:eventmanager.eventless",
                            id
                        )
                    );

                    break;
                }

                // assign the job to existing applications
                foreach (var applicationContext in _componentHub.ApplicationManager.GetApplications(pluginContext))
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
            }
        }

        /// <summary>
        /// Removes all jobs associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the event to remove.</param>
        internal void Remove(IPluginContext pluginContext)
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
        /// Removes all jobs associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the jobs to remove.</param>
        internal void Remove(IApplicationContext applicationContext)
        {
            if (applicationContext == null)
            {
                return;
            }

            foreach (var pluginDict in _dictionary.Values)
            {
                foreach (var appDict in pluginDict.Where(x => x.Key == applicationContext).Select(x => x.Value))
                {
                    foreach (var eventHandlerItem in appDict.Values.SelectMany(x => x))
                    {
                        OnRemoveEventHandler(eventHandlerItem.EventHandlerContext);
                        eventHandlerItem.Dispose();
                    }
                }

                pluginDict.Remove(applicationContext);
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
        /// Handles the event when an plugin is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the plugin being added.</param>
        private void OnAddPlugin(object sender, IPluginContext e)
        {
            Register(e);
        }

        /// <summary>  
        /// Handles the event when a plugin is removed.  
        /// </summary>  
        /// <param name="sender">The source of the event.</param>  
        /// <param name="e">The context of the plugin being removed.</param>  
        private void OnRemovePlugin(object sender, IPluginContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Handles the event when an application is removed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being removed.</param>
        private void OnRemoveApplication(object sender, IApplicationContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Handles the event when an application is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being added.</param>
        private void OnAddApplication(object sender, IApplicationContext e)
        {
            Register(e);
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

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
            _componentHub.PluginManager.AddPlugin -= OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin -= OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication -= OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication -= OnRemoveApplication;
        }
    }
}
