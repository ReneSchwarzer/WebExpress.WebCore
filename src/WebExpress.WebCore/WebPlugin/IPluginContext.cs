using System.Reflection;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;

namespace WebExpress.WebCore.WebPlugin
{
    /// <summary>
    /// The context of a plugin.
    /// </summary>
    public interface IPluginContext : IContext
    {
        /// <summary>
        /// Gets the assembly that contains the plugin.
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        /// Gets the plugin id.
        /// </summary>
        IComponentId PluginId { get; }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        string PluginName { get; }

        /// <summary>
        /// Gets the manufacturer of the plugin.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the version of the plugin.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the copyright information.
        /// </summary>
        string Copyright { get; }

        /// <summary>
        /// Gets the license information.
        /// </summary>
        string License { get; }

        /// <summary>
        /// Gets the icon of the plugin.
        /// </summary>
        IRoute Icon { get; }
    }
}
