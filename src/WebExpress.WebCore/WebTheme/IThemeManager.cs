using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebTheme
{
    /// <summary>
    /// Interface for managing themes within the web application.
    /// </summary>
    public interface IThemeManager : IComponentManager
    {
        /// <summary>
        /// An event that fires when a theme is added.
        /// </summary>
        event EventHandler<IThemeContext> AddTheme;

        /// <summary>
        /// An event that fires when a theme is removed.
        /// </summary>
        event EventHandler<IThemeContext> RemoveTheme;

        /// <summary>
        /// Returns the collection of themes.
        /// </summary>
        IEnumerable<IThemeContext> Themes { get; }

        /// <summary>
        /// Returns the theme contexts.
        /// </summary>
        /// <typeparam name="TTheme">The type of theme.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of theme contexts.</returns>
        IEnumerable<IThemeContext> GetThemes<TTheme>(IApplicationContext applicationContext)
            where TTheme : ITheme;

        /// <summary>
        /// Returns the theme contexts.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="themeType">The type of theme.</param>
        /// <returns>An IEnumerable of theme contexts.</returns>
        IEnumerable<IThemeContext> GetThemes(IApplicationContext applicationContext, Type themeType);

        /// <summary>
        /// Returns the theme associated with the specified theme context.
        /// </summary>
        /// <param name="themeContext">The context of the theme to retrieve.</param>
        /// <returns>The theme associated with the specified context.</returns>
        ITheme GetTheme(IThemeContext themeContext);
    }
}
