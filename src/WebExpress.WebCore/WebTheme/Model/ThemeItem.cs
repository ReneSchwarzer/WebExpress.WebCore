using System;

namespace WebExpress.WebCore.WebTheme.Model
{
    /// <summary>
    /// Represents an theme item.
    /// </summary>
    public class ThemeItem : IDisposable
    {
        /// <summary>
        /// Gets or sets the type of theme.
        /// </summary>
        public Type ThemeClass { get; set; }

        /// <summary>
        /// Gets or sets the instance of the theme.
        /// </summary>
        public ITheme Instance { get; set; }

        /// <summary>
        /// Gets the theme context.
        /// </summary>
        public IThemeContext ThemeContext { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        internal ThemeItem()
        {
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Convert the theme element to a string.
        /// </summary>
        /// <returns>The theme element in its string representation.</returns>
        public override string ToString()
        {
            return $"Theme: '{ThemeContext?.ThemeId}'";
        }
    }
}
