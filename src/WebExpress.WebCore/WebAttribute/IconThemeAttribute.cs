using System;
using WebExpress.WebCore.WebIcon;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies the icon theme to use for rendering icons within a theme.
    /// Apply this attribute to a class implementing <c>ITheme</c>; the value is
    /// surfaced through <c>IThemeContext.IconTheme</c> and emitted on the root
    /// <c>&lt;html data-icon-theme&gt;</c> attribute when the theme is active.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IconThemeAttribute : Attribute, IThemeAttribute
    {
        /// <summary>
        /// Gets the icon theme used to display type icons.
        /// </summary>
        public TypeIconTheme Theme { get; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public IconThemeAttribute()
        {
            Theme = TypeIconTheme.Default;
        }

        /// <summary>
        /// Initializes a new instance of the class using the specified icon theme.
        /// </summary>
        /// <param name="theme">
        /// The icon theme applied to this attribute. The selected theme determines the visual appearance
        /// of the associated web icon.
        /// </param>

        public IconThemeAttribute(TypeIconTheme theme)
        {
            Theme = theme;
        }
    }
}
