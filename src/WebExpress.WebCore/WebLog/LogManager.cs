using System;
using System.Collections.Generic;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebLog
{
    public class LogManager : IComponentPlugin, ISystemComponent
    {
        /// <summary>
        /// An event that fires when an log is added.
        /// </summary>
        public event EventHandler<IPluginContext> AddLog;

        /// <summary>
        /// An event that fires when an log is removed.
        /// </summary>

        public event EventHandler<IPluginContext> RemoveLog;

        /// <summary>
        /// Returns or sets the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns the default log.
        /// </summary>
        public ILog DefaultLog => HttpServerContext.Log;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager"/> class.
        /// </summary>
        internal LogManager()
        {
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
                InternationalizationManager.I18N("webexpress:logmanager.initialization")
            );
        }

        /// <summary>
        /// Discovers and registers logs from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose logs are to be registered.</param>
        public void Register(IPluginContext pluginContext)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Discovers and registers logs from the specified plugin.
        /// </summary>
        /// <param name="pluginContexts">A list with plugin contexts that contain the logs.</param>
        public void Register(IEnumerable<IPluginContext> pluginContexts)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Removes all logs associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the log to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Raises the AddLog event.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        private void OnAddModule(IPluginContext pluginContext)
        {
            AddLog?.Invoke(this, pluginContext);
        }

        /// <summary>
        /// Raises the RemoveLog event.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        private void OnRemoveModule(IPluginContext pluginContext)
        {
            RemoveLog?.Invoke(this, pluginContext);
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
            output.Add
            (
                string.Empty.PadRight(deep) +
                InternationalizationManager.I18N("webexpress:logmanager.titel")
            );
        }
    }
}
