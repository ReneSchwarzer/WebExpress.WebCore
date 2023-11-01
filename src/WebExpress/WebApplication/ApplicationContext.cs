using System.Collections.Generic;
using WebExpress.WebPlugin;
using WebExpress.WebUri;

namespace WebExpress.WebApplication
{
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
        /// Returns an enumeration of options. Options enable optional resources.
        /// </summary>
        public IEnumerable<string> Options { get; internal set; }

        /// <summary>
        /// Returns the asset directory. This is mounted in the asset directory of the server.
        /// </summary>
        public string AssetPath { get; internal set; }

        /// <summary>
        /// Returns the data directory. This is mounted in the data directory of the server.
        /// </summary>
        public string DataPath { get; internal set; }

        /// <summary>
        /// Returns the context path. This is mounted in the context path of the server.
        /// </summary>
        public UriResource ContextPath { get; internal set; }

        /// <summary>
        /// Returns the icon uri.
        /// </summary>
        public UriResource Icon { get; internal set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationContext()
        {
        }

        /// <summary>
        /// Conversion of the apllication context into its string representation.
        /// </summary>
        /// <returns>The string that uniquely represents the application.</returns>
        public override string ToString()
        {
            return $"Application {ApplicationId}";
        }
    }
}
