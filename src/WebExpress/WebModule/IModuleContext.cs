using WebExpress.WebApplication;
using WebExpress.WebPlugin;
using WebExpress.WebUri;

namespace WebExpress.WebModule
{
    public interface IModuleContext
    {
        /// <summary>
        /// Returns the context of the associated plugin.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the associated application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Returns the modul id.
        /// </summary>
        string ModuleId { get; }

        /// <summary>
        /// Returns the module name.
        /// </summary>
        string ModuleName { get; }

        /// <summary>
        /// Returns the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Returns the asset directory.
        /// </summary>
        string AssetPath { get; }

        /// <summary>
        /// Returns the data directory.
        /// </summary>
        string DataPath { get; }

        /// <summary>
        /// Returns the context path.
        /// </summary>
        UriResource ContextPath { get; }

        /// <summary>
        /// Returns the icon uri.
        /// </summary>
        UriResource Icon { get; }
    }
}
