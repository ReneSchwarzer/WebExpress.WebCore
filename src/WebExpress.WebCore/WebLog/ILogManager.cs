using System;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebLog
{
    /// <summary>
    /// Interface for managing logs within the web application.
    /// </summary>
    public interface ILogManager : IComponentManager
    {
        /// <summary>
        /// An event that fires when a log is added.
        /// </summary>
        event EventHandler<IPluginContext> AddLog;

        /// <summary>
        /// An event that fires when a log is removed.
        /// </summary>
        event EventHandler<IPluginContext> RemoveLog;

        /// <summary>
        /// Gets the default log.
        /// </summary>
        ILog DefaultLog { get; }
    }
}
