using System;
using WebExpress.WebCore.WebTheme;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to specify the theme mode.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ThemeModeAttribute : Attribute, IThemeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class with the specified theme mode.
        /// </summary>
        /// <param name="mode">The theme mode to be applied.</param>
        public ThemeModeAttribute(ThemeMode mode)
        {

        }
    }
}
