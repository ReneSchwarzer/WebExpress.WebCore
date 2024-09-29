using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebModule
{
    public class ModuleContext : IModuleContext
    {
        /// <summary>
        /// Returns the context of the associated plugin.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the associated application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns the modul id.
        /// </summary>
        public string ModuleId { get; internal set; }

        /// <summary>
        /// Returns the module name.
        /// </summary>
        public string ModuleName { get; internal set; }

        /// <summary>
        /// Returns the description.
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Returns the asset directory.
        /// </summary>
        public string AssetPath { get; internal set; }

        /// <summary>
        /// Returns the data directory. 
        /// </summary>
        public string DataPath { get; internal set; }

        /// <summary>
        /// Returns the context path.
        /// </summary>
        public UriResource ContextPath { get; internal set; }

        /// <summary>
        /// Returns the icon uri.
        /// </summary>
        public UriResource Icon { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        internal ModuleContext()
        {
        }

        /// <summary>
        /// Conversion of the module context into its string representation.
        /// </summary>
        /// <returns>The string that uniquely represents the module.</returns>
        public override string ToString()
        {
            return $"{ApplicationContext?.ApplicationId}:{ModuleId}";
        }
    }
}
