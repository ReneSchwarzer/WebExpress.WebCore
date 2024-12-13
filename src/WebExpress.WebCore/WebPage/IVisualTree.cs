using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents a visual tree for rendering a web page.
    /// </summary>
    public interface IVisualTree
    {
        /// <summary>
        /// Convert to html.
        /// </summary>
        /// <param name="context">The context for rendering the visual tree.</param>
        /// <returns>The page as html.</returns>
        IHtmlNode Render(IVisualTreeContext context);
    }
}
