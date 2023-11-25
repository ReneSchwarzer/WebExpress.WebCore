using System.Reflection;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPlugin
{
    public class PluginContext : IPluginContext
    {
        /// <summary>
        /// The assembly that contains the plugin.
        /// </summary>
        public Assembly Assembly { get; internal set; }

        /// <summary>
        /// Returns the plugin id.
        /// </summary>
        public string PluginId { get; internal set; }

        /// <summary>
        /// Returns the name of the plugin.
        /// </summary>
        public string PluginName { get; internal set; }

        /// <summary>
        /// Returns the manufacturer of the plugin.
        /// </summary>
        public string Manufacturer { get; internal set; }

        /// <summary>
        /// Returns the copyright information.
        /// </summary>
        public string Copyright { get; internal set; }

        /// <summary>
        /// Returns the description of the plugin.
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Returns the version of the plugin.
        /// </summary>
        public string Version { get; internal set; }

        /// <summary>
        /// Returns the license information.
        /// </summary>
        public string License { get; internal set; }

        /// <summary>
        /// Returns the icon of the plugin.
        /// </summary>
        public UriResource Icon { get; internal set; }

        /// <summary>
        /// Returns the host context.
        /// </summary>
        public IHttpServerContext Host { get; internal set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PluginContext()
        {
        }

        /// <summary>
        /// Conversion of the plugin context into its string representation.
        /// </summary>
        /// <returns>The string that uniquely represents the plugin.</returns>
        public override string ToString()
        {
            return PluginId;
        }
    }
}
