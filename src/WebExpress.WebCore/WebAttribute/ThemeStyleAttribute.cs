using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies the style for a theme.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ThemeStyleAttribute : Attribute, IThemeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class with the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the css theme style.</param>
        public ThemeStyleAttribute(string uri)
        {

        }
    }
}
