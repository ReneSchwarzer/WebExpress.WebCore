using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebInclude
{
    /// <summary>
    /// The include context describes an include registration per plugin and application including files, kind, and scopes.
    /// </summary>
    public interface IIncludeContext : IContext
    {
        /// <summary>
        /// Retruns the identifier of the included component.
        /// </summary>
        ComponentId IncludeId { get; }

        /// <summary>
        /// Retruns the context for the plugin, providing access to shared resources and services.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the application context that provides configuration and services for the application.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Returns a value indicating whether caching is enabled.
        /// </summary>
        bool Cache { get; }

        /// <summary>
        /// Returns the collection of files to be included.
        /// </summary>
        IEnumerable<IncludeFile> Files { get; }

        /// <summary>
        /// Retruns the collection of scopes associated with the current context.
        /// </summary>
        /// <remarks>
        /// The collection can be empty if no scopes are defined. Callers can set this property
        /// to customize the applicable scopes or retrieve it to inspect the current configuration.
        /// </remarks>
        IEnumerable<Type> Scopes { get; }
    }
}
