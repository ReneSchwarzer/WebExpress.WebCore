using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebTheme;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy theme for testing.
    /// </summary>
    [Name("TestThemeB")]
    public sealed class TestThemeB : ITheme
    {
        /// <summary>
        /// Returns the text color for the theme.
        /// </summary>
        /// <value>
        /// A string representing the text color in hexadecimal format.
        /// </value>
        public static string TextColor => "FFFFFF";
    }
}
