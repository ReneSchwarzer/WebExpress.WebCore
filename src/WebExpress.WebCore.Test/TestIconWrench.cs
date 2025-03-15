
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebIcon;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// Represents a test icon that can be rendered to HTML.
    /// </summary>
    public class TestIconWrench : IIcon
    {
        /// <summary>
        /// Converts the icon to an HTML representation.
        /// </summary>
        /// <param name="renderContext">The context in which the icon is rendered.</param>
        /// <param name="visualTree">The visual tree representing the icon's structure.</param>
        /// <returns>An HTML node representing the rendered icon.</returns>
        public IHtmlNode Render(IRenderContext renderContext, IVisualTree visualTree)
        {
            return new HtmlElementTextSemanticsSpan(new HtmlText("🔧"));
        }
    }
}
