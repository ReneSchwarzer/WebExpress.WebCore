using System.Collections.Generic;
using WebExpress.WebPlugin;
using WebExpress.WebUri;

namespace WebExpress.WebApplication
{
    /// <summary>
    /// The application context.
    /// </summary>
    public interface IApplicationContext
    {
        /// <summary>
        /// Provides the context of the associated plugin.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the application id.
        /// </summary>
        string ApplicationId { get; }

        /// <summary>
        /// Returns the application name.
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// Provides the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Returns an enumeration of options. Options enable optional resources.
        /// </summary>
        IEnumerable<string> Options { get; }

        /// <summary>
        /// Returns the asset directory. This is mounted in the asset directory of the server.
        /// </summary>
        string AssetPath { get; }

        /// <summary>
        /// Returns the data directory. This is mounted in the data directory of the server.
        /// </summary>
        string DataPath { get; }

        /// <summary>
        /// Returns the context path. This is mounted in the context path of the server.
        /// </summary>
        UriResource ContextPath { get; }

        /// <summary>
        /// Returns the icon uri.
        /// </summary>
        UriResource Icon { get; }
    }
}
