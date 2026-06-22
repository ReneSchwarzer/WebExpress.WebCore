using System.Reflection;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;

namespace WebExpress.WebCore.WebPlugin
{
    /// <summary>
    /// Read-only descriptor of a loaded plugin that the framework passes around so components can
    /// learn which plugin they came from and read its metadata — id, name, manufacturer, version,
    /// description, copyright, license, icon, and the .NET assembly it lives in.
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
