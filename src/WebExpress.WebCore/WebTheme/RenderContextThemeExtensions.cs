using System.Linq;
using WebExpress.WebCore.WebIcon;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebTheme
{
    /// <summary>
    /// Render-time extensions that resolve the active theme for the
    /// application carried in the render context.
    /// </summary>
    /// <remarks>
    /// Themes were moved out of <c>IApplicationContext</c> with the icon-theme
    /// migration: the active theme is now the first theme registered for an
    /// application via the <c>ThemeManager</c>. These helpers keep call sites
    /// short for code that has a render context but no access to the visual
    /// tree (e.g. <c>Icon</c> Funcs on form buttons).
    /// </remarks>
    public static class RenderContextThemeExtensions
    {
        /// <summary>
        /// Returns the first theme registered for the render context's
        /// application, or <see langword="null"/> when no theme has been
        /// registered.
        /// </summary>
        /// <param name="renderContext">The current render context.</param>
        /// <returns>The active theme context or <see langword="null"/>.</returns>
        public static IThemeContext GetActiveTheme(this IRenderContext renderContext)
        {
            var applicationContext = renderContext?.PageContext?.ApplicationContext;
            if (applicationContext is null)
            {
                return null;
            }

            return WebEx.ComponentHub?.ThemeManager?.Themes
                ?.FirstOrDefault(t => t.ApplicationContext == applicationContext);
        }

        /// <summary>
        /// Returns the icon theme of the active theme, falling back to
        /// <see cref="TypeIconTheme.Default"/> when no theme is registered
        /// for the render context's application.
        /// </summary>
        /// <param name="renderContext">The current render context.</param>
        /// <returns>The icon theme to use when rendering icons.</returns>
        public static TypeIconTheme GetIconTheme(this IRenderContext renderContext)
        {
            return renderContext.GetActiveTheme()?.IconTheme ?? TypeIconTheme.Default;
        }
    }
}
