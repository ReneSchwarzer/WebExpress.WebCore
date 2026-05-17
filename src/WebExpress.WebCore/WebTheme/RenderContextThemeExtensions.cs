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
    /// The resolution order mirrors <c>VisualTreeControl</c> so server-side
    /// icon factories and the visual tree end up with the same theme:
    /// <list type="number">
    ///   <item><description>
    ///     The application's declared default theme (<c>[Theme&lt;T&gt;]</c> →
    ///     <c>IApplicationContext.DefaultTheme</c>).
    ///   </description></item>
    ///   <item><description>
    ///     The first theme registered for the application (legacy fallback).
    ///   </description></item>
    ///   <item><description>
    ///     <see langword="null"/>; downstream <see cref="TypeIconTheme"/>
    ///     callers fall back to <see cref="TypeIconTheme.Default"/>.
    ///   </description></item>
    /// </list>
    /// Per-user overrides are wired by application code: the page's
    /// <c>Process</c> hook calls <c>visualTree.UseTheme&lt;TTheme&gt;()</c>
    /// based on whatever store the application keeps; the framework itself
    /// does not consult cookies, sessions, or identities.
    /// </remarks>
    public static class RenderContextThemeExtensions
    {
        /// <summary>
        /// Returns the active theme for the render context using the
        /// resolution order documented on the class.
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

            // 1. application's declared default
            if (applicationContext.DefaultTheme is { } declared)
            {
                return declared;
            }

            // 2. first registered theme for the application
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

        /// <summary>
        /// Re-themes an existing <see cref="WebIcon.IIcon"/> for the active
        /// icon theme of <paramref name="renderContext"/>. Convenience over
        /// <see cref="ApplyIconTheme(WebIcon.IIcon, TypeIconTheme)"/> for
        /// callers that only have a render context in hand.
        /// </summary>
        /// <param name="icon">The icon to re-theme; may be <see langword="null"/>.</param>
        /// <param name="renderContext">The current render context.</param>
        /// <returns>The re-themed icon or the original instance.</returns>
        public static WebIcon.IIcon ApplyIconTheme(this WebIcon.IIcon icon, IRenderContext renderContext)
        {
            return icon.ApplyIconTheme(renderContext.GetIconTheme());
        }

        /// <summary>
        /// Re-themes an existing <see cref="WebIcon.IIcon"/> for the given
        /// <paramref name="theme"/>. Icons created at registration time
        /// (e.g. <c>PageContext.PageIcon</c>) carry the theme that was
        /// active when the page was discovered; this helper rebuilds them
        /// so the breadcrumb, sidebars, etc. swap glyphs at runtime when
        /// the application code activates a different theme via
        /// <c>visualTree.UseTheme&lt;TTheme&gt;()</c>. Controls that have
        /// a visual tree in hand should pass
        /// <c>visualTree.IconTheme</c> here.
        /// <para>
        /// Falls back to <paramref name="icon"/> when its concrete type does
        /// not expose a <c>(TypeIconTheme)</c> constructor.
        /// </para>
        /// </summary>
        /// <param name="icon">The icon to re-theme; may be <see langword="null"/>.</param>
        /// <param name="theme">The icon theme to apply.</param>
        /// <returns>The re-themed icon or the original instance.</returns>
        public static WebIcon.IIcon ApplyIconTheme(this WebIcon.IIcon icon, TypeIconTheme theme)
        {
            if (icon is null)
            {
                return null;
            }

            var iconType = icon.GetType();
            var ctor = iconType.GetConstructor(new[] { typeof(TypeIconTheme) });
            if (ctor is null)
            {
                return icon;
            }

            try
            {
                return ctor.Invoke(new object[] { theme }) as WebIcon.IIcon ?? icon;
            }
            catch
            {
                return icon;
            }
        }
    }
}
