using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebApplication
{
    /// <summary>
    /// Represents the context of an application.
    /// </summary>
    public class ApplicationContext : IApplicationContext
    {
        /// <summary>
        /// Returns the context of the associated plugin.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the application id.
        /// </summary>
        public string ApplicationId { get; internal set; }

        /// <summary>
        /// Returns the application name.
        /// </summary>
        public string ApplicationName { get; internal set; }

        /// <summary>
        /// Returns or sets the description.
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Returns the asset directory. This is mounted in the asset directory of the server.
        /// </summary>
        public string AssetPath { get; internal set; }

        /// <summary>
        /// Returns the data directory. This is mounted in the data directory of the server.
        /// </summary>
        public string DataPath { get; internal set; }

        /// <summary>
        /// Returns the context path. This is mounted in the route of the server.
        /// </summary>
        public IRoute Route { get; internal set; }

        /// <summary>
        /// Returns the icon uri.
        /// </summary>
        public IRoute Icon { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ApplicationContext()
        {
        }

        /// <summary>
        /// Conversion of the application context into its string representation.
        /// </summary>
        /// <returns>The string that uniquely represents the application.</returns>
        public override string ToString()
        {
            return $"Application: {ApplicationId}";
        }
    }
}
