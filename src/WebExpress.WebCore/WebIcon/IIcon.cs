using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebIcon
{
    /// <summary>
    /// Represents an icon that can be rendered to HTML.
    /// </summary>
    public interface IIcon
    {
        /// <summary>
        /// Converts the icon to an HTML representation.
        /// </summary>
        /// <param name="renderContext">The context in which the icon is rendered.</param>
        /// <param name="visualTree">The visual tree representing the icon's structure.</param>
        /// <returns>An HTML node representing the rendered icon.</returns>
        IHtmlNode Render(IRenderContext renderContext, IVisualTree visualTree);
    }
}
