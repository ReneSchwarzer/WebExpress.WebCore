using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebTheme;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy theme for testing.
    /// </summary>
    [Name("TestThemeA")]
    [Description("A dummy theme for testing.")]
    [Image("webexpress.webcore.test.testthemea.png")]
    public sealed class TestThemeA : ITheme
    {
        /// <summary>
        /// Returns the text color for the theme.
        /// </summary>
        /// <value>
        /// A string representing the text color in hexadecimal format.
        /// </value>
        public static string TextColor => "000000";
    }
}
