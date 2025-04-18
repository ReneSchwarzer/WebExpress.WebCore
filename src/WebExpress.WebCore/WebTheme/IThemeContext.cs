using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebTheme
{
    /// <summary>
    /// Represents the context for a theme in the web application.
    /// </summary>
    public interface IThemeContext : IContext
    {
        /// <summary>
        /// Returns the theme id.
        /// </summary>
        IComponentId ThemeId { get; }

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Returns the image associated with the theme.
        /// </summary>
        IRoute Image { get; }

        /// <summary>
        /// Returns the name of the theme.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the description of the theme.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Returns the mode of the theme.
        /// </summary>
        ThemeMode ThemeMode { get; }

        /// <summary>
        /// Returns the route resource for the css theme style.
        /// </summary>
        IRoute ThemeStyle { get; }
    }
}
