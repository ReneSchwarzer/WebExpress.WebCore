using System.Reflection;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPlugin
{
    /// <summary>
    /// The context of a plugin.
    /// </summary>
    public interface IPluginContext : IContext
    {
        /// <summary>
        /// The assembly that contains the plugin.
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        /// Returns the plugin id.
        /// </summary>
        string PluginId { get; }

        /// <summary>
        /// Returns the name of the plugin.
        /// </summary>
        string PluginName { get; }

        /// <summary>
        /// Returns the manufacturer of the plugin.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        /// Returns the description of the plugin.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Returns the version of the plugin.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Returns the copyright information.
        /// </summary>
        string Copyright { get; }

        /// <summary>
        /// Returns the license information.
        /// </summary>
        string License { get; }

        /// <summary>
        /// Returns the icon of the plugin.
        /// </summary>
        UriResource Icon { get; }

        /// <summary>
        /// Returns the host context.
        /// </summary>
        IHttpServerContext Host { get; }
    }
}
