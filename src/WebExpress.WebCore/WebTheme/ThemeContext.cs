using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebTheme
{
    /// <summary>
    /// Represents the context for a theme in the web application.
    /// </summary>
    public class ThemeContext : IThemeContext
    {
        /// <summary>
        /// Returns the theme id.
        /// </summary>
        public IComponentId ThemeId { get; internal set; }

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns the image associated with the theme.
        /// </summary>
        public UriResource Image { get; internal set; }

        /// <summary>
        /// Returns the name of the theme.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Returns the description of the theme.
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Returns the mode of the theme.
        /// </summary>
        public ThemeMode ThemeMode { get; internal set; }

        /// <summary>
        /// Returns the URI resource for the css theme style.
        /// </summary>
        public UriResource ThemeStyle { get; internal set; }
    }
}
