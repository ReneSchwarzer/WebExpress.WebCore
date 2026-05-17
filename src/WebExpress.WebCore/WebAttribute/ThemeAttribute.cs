using System;
using WebExpress.WebCore.WebTheme;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Declares the default theme of an application. Apply this attribute to a
    /// class implementing <c>IApplication</c>; the type argument is the theme
    /// class to use. <see cref="WebApplication.IApplicationContext.DefaultTheme"/>
    /// resolves the matching <c>IThemeContext</c> via the active
    /// <c>ThemeManager</c> on each read.
    /// </summary>
    /// <typeparam name="TTheme">The theme type to use as the application default.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ThemeAttribute<TTheme> : Attribute, IApplicationAttribute
        where TTheme : class, ITheme
    {
    }
}
