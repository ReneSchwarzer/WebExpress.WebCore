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
        /// Gets the context of the associated plugin.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Gets the application id.
        /// </summary>
        string ApplicationId { get; }

        /// <summary>
        /// Gets the application name.
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the asset directory. This is mounted in the asset directory of the server.
        /// </summary>
        string AssetPath { get; }

        /// <summary>
        /// Gets the data directory. This is mounted in the data directory of the server.
        /// </summary>
        string DataPath { get; }

        /// <summary>
        /// Gets the context path. This is mounted in the route of the server.
        /// </summary>
        IRoute Route { get; }

        /// <summary>
        /// Gets the icon uri.
        /// </summary>
        IRoute Icon { get; }
    }
}
