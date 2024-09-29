using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebApplication
{
    /// <summary>
    /// Interface of the management of WebExpress applications.
    /// </summary>
    public interface IApplicationManager : IManager
    {
        /// <summary>
        /// An event that fires when an application is added.
        /// </summary>
        event EventHandler<IApplicationContext> AddApplication;

        /// <summary>
        /// An event that fires when an application is removed.
        /// </summary>
        event EventHandler<IApplicationContext> RemoveApplication;

        /// <summary>
        /// Returns the stored applications.
        /// </summary>
        IEnumerable<IApplicationContext> Applications { get; }

        /// <summary>
        /// Determines the application contexts for a given application id.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns>The context of the application or null.</returns>
        IApplicationContext GetApplcation(string applicationId);

        /// <summary>
        /// Determines the application contexts for a given application id.
        /// </summary>
        /// <param name="application">The application type.</param>
        /// <returns>The context of the application or null.</returns>
        IApplicationContext GetApplcation(Type application);

        /// <summary>
        /// Determines the application contexts for the given application ids.
        /// </summary>
        /// <param name="applicationIds">The applications ids. Can contain regular expressions or * for all.</param>
        /// <returns>The contexts of the applications as an enumeration.</returns>
        IEnumerable<IApplicationContext> GetApplcations(IEnumerable<string> applicationIds);

        /// <summary>
        /// Determines the application contexts for the given plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <returns>The contexts of the applications as an enumeration.</returns>
        IEnumerable<IApplicationContext> GetApplcations(IPluginContext pluginContext);
    }
}
