﻿using System;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebEvent.Model
{
    /// <summary>
    /// Represents an event item.
    /// This class is intended for internal use only.
    /// </summary>
    internal class EventItem : IDisposable
    {
        private readonly IComponentHub _componentHub;
        private readonly IDisposable _instance;

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; private set; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; private set; }

        /// <summary>
        /// Returns or sets the event handler context.
        /// </summary>
        public IEventHandlerContext EventHandlerContext { get; private set; }

        /// <summary>
        /// Returns or sets the event class.
        /// </summary>
        public Type EventClass { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The associated component hub.</param>
        /// <param name="pluginContext">The associated plugin context.</param>
        /// <param name="applicationContext">The corresponding application context.</param>
        /// <param name="eventHandlerContext">The event handler context.</param>
        /// <param name="eventHandlerType">The event handler type.</param>
        /// <param name="eventClass">The event class.</param>
        public EventItem(IComponentHub componentHub, IPluginContext pluginContext, IApplicationContext applicationContext, IEventHandlerContext eventHandlerContext, Type eventHandlerType, Type eventClass)
        {
            _componentHub = componentHub;
            PluginContext = pluginContext;
            ApplicationContext = applicationContext;
            EventHandlerContext = eventHandlerContext;
            EventClass = eventClass;

            if (typeof(IEventHandler).IsAssignableFrom(eventHandlerType))
            {
                _instance = ComponentActivator.CreateInstance<IEventHandler, IEventHandlerContext>
                 (
                     eventHandlerType,
                     eventHandlerContext,
                     _componentHub,
                     pluginContext,
                     applicationContext
                 );

                return;
            }

            _instance = ComponentActivator.CreateInstance<IEventHandlerContext>
            (
                eventHandlerType,
                eventHandlerContext,
                _componentHub,
                pluginContext,
                applicationContext
            );
        }

        /// <summary>
        /// Process the event.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="eventArgument">The argument for the event.</param>
        public void Process(object sender, IEventArgument eventArgument)
        {
            if (_instance is IEventHandler handler)
            {
                handler.Process(sender, eventArgument);

                return;
            }

            var handlerType = _instance.GetType()
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));

            if (handlerType != null)
            {
                var genericArgument = handlerType.GetGenericArguments().First();
                var method = handlerType.GetMethod("Process");
                method.Invoke(_instance, [sender, eventArgument]);
            }
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _instance?.Dispose();
        }

        /// <summary>
        /// Convert the resource element to a string.
        /// </summary>
        /// <returns>The event element in its string representation.</returns>
        public override string ToString()
        {
            return $"Event '{EventClass.FullName.ToLower()}'";
        }
    }
}
