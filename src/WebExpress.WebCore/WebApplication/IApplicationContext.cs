using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebApplication
{
    /// <summary>
    /// The application context.
    /// </summary>
    public interface IApplicationContext : IContext
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
        /// Returns the asset directory. This is mounted in the asset directory of the server.
        /// </summary>
        string AssetPath { get; }

        /// <summary>
        /// Returns the data directory. This is mounted in the data directory of the server.
        /// </summary>
        string DataPath { get; }

        /// <summary>
        /// Returns the context path. This is mounted in the route of the server.
        /// </summary>
        IRoute Route { get; }

        /// <summary>
        /// Returns the icon uri.
        /// </summary>
        IRoute Icon { get; }
    }
}
