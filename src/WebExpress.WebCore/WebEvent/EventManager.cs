using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEvent.Model;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebEvent
{
    /// <summary>
    /// The event manager.
    /// </summary>
    public sealed class EventManager : IComponentManagerPlugin, ISystemComponent
    {
        private readonly IComponentHub _componentManager;
        private readonly IHttpServerContext _httpServerContext;
        private readonly EventDictionary _dictionary = [];

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentManager">The component manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        internal EventManager(IComponentHub componentManager, IHttpServerContext httpServerContext)
        {
            _componentManager = componentManager;

            _componentManager.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            _componentManager.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };

            _componentManager.ModuleManager.AddModule += (sender, moduleContext) =>
            {
                AssignToModule(moduleContext);
            };

            _componentManager.ModuleManager.RemoveModule += (sender, moduleContext) =>
            {
                DetachFromModule(moduleContext);
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
                    x.GetInterface(typeof(IEventHandler).Name) != null
                ))
            {

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
        /// Assign existing event to the module.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        private void AssignToModule(IModuleContext moduleContext)
        {
            //foreach (var scheduleItem in _dictionary.Values.SelectMany(x => x))
            //{
            //    if (scheduleItem.moduleId.Equals(moduleContext?.ModuleId))
            //    {
            //        scheduleItem.AddModule(moduleContext);
            //    }
            //}
        }

        /// <summary>
        /// Remove an existing modules to the event.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        private void DetachFromModule(IModuleContext moduleContext)
        {
            //foreach (var scheduleItem in _dictionary.Values.SelectMany(x => x))
            //{
            //    if (scheduleItem.moduleId.Equals(moduleContext?.ModuleId))
            //    {
            //        scheduleItem.DetachModule(moduleContext);
            //    }
            //}
        }

        /// <summary>
        /// Removes all jobs associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the event to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            //// the plugin has not been registered in the manager
            //if (!_dictionary.ContainsKey(pluginContext))
            //{
            //    return;
            //}

            //foreach (var scheduleItem in _dictionary[pluginContext])
            //{
            //    scheduleItem.Dispose();
            //}

            //_dictionary.Remove(pluginContext);
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
