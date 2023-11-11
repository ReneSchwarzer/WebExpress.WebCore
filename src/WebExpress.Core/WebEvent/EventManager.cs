using System.Collections.Generic;
using System.Linq;
using WebExpress.Core.Internationalization;
using WebExpress.Core.WebComponent;
using WebExpress.Core.WebJob;
using WebExpress.Core.WebModule;
using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebEvent
{
    /// <summary>
    /// The event manager.
    /// </summary>
    public sealed class EventManager : IComponentPlugin, ISystemComponent
    {
        /// <summary>
        /// Returns or sets the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns the directory where the events are listed.
        /// </summary>
        private EventDictionary Dictionary { get; } = new EventDictionary();

        /// <summary>
        /// Constructor
        /// </summary>
        internal EventManager()
        {
            ComponentManager.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            ComponentManager.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };

            ComponentManager.ModuleManager.AddModule += (sender, moduleContext) =>
            {
                AssignToModule(moduleContext);
            };

            ComponentManager.ModuleManager.RemoveModule += (sender, moduleContext) =>
            {
                DetachFromModule(moduleContext);
            };
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The reference to the context of the host.</param>
        public void Initialization(IHttpServerContext context)
        {
            HttpServerContext = context;

            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N
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
            //foreach (var scheduleItem in Dictionary.Values.SelectMany(x => x))
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
            //foreach (var scheduleItem in Dictionary.Values.SelectMany(x => x))
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
            //if (!Dictionary.ContainsKey(pluginContext))
            //{
            //    return;
            //}

            //foreach (var scheduleItem in Dictionary[pluginContext])
            //{
            //    scheduleItem.Dispose();
            //}

            //Dictionary.Remove(pluginContext);
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
            //        InternationalizationManager.I18N
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
