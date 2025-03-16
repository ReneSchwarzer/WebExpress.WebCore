using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebIcon;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// Represents a test icon that can be rendered to HTML.
    /// </summary>
    public class TestIconProfile : IIcon
    {
        /// <summary>
        /// Converts the icon to an HTML representation.
        /// </summary>
        /// <param name="renderContext">The context in which the icon is rendered.</param>
        /// <param name="visualTree">The visual tree representing the icon's structure.</param>
        /// <param name="id">The id attribute of the HTML element.</param>
        /// <param name="description">The description of the icon.</param>
        /// <param name="css">The CSS class of the HTML element.</param>
        /// <param name="style">The inline style of the HTML element.</param>
        /// <param name="role">The ARIA role of the HTML element.</param>
        /// <returns>An HTML node representing the rendered icon.</returns>
        public IHtmlNode Render(IRenderContext renderContext, IVisualTree visualTree, string id = null, string description = null, string css = null, string style = null, string role = null)
        {
            return new HtmlElementTextSemanticsSpan(new HtmlText("👤"));
        }
    }
}
