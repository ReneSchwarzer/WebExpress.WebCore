using System;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebLog
{
    /// <summary>
    /// Manages logging operations and integrates with the system components.
    /// </summary>
    public class LogManager : ILogManager, ISystemComponent
    {
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;

        /// <summary>
        /// An event that fires when a log is added.
        /// </summary>
        public event EventHandler<IPluginContext> AddLog;

        /// <summary>
        /// An event that fires when a log is removed.
        /// </summary>

        public event EventHandler<IPluginContext> RemoveLog;

        /// <summary>
        /// Returns the default log.
        /// </summary>
        public ILog DefaultLog => _httpServerContext.Log;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private LogManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;
            _httpServerContext = httpServerContext;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress:logmanager.initialization")
            );
        }

        /// <summary>
        /// Discovers and registers logs from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose logs are to be registered.</param>
        private void Register(IPluginContext pluginContext)
        {

        }

        /// <summary>
        /// Removes all logs associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the log to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
        }

        /// <summary>
        /// Raises the AddLog event.
        /// </summary>
        /// <param name="resourceContext">The page context.</param>
        private void OnAddLog(IPluginContext resourceContext)
        {
            AddLog?.Invoke(this, resourceContext);
        }

        /// <summary>
        /// Raises the RemoveLog event.
        /// </summary>
        /// <param name="pluginContext">The page context.</param>
        private void OnRemoveLog(IPluginContext pluginContext)
        {
            RemoveLog?.Invoke(this, pluginContext);
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
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
            _componentHub.PluginManager.AddPlugin -= OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin -= OnRemovePlugin;

            GC.SuppressFinalize(this);
        }
    }
}
