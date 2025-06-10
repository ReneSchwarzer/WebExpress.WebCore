using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents a visual tree for rendering a web page.
    /// </summary>
    public interface IVisualTree
    {
        /// <summary>
        /// Converts to an HTML representation.
        /// </summary>
        /// <param name="context">The context for rendering the visual tree.</param>
        /// <returns>The page as html.</returns>
        IHtmlNode Render(IVisualTreeContext context);

        /// <summary>
        /// Retrieves a response based on the provided visual tree context.
        /// </summary>
        /// <param name="context">The visual tree context used to generate the response. Cannot be null.</param>
        /// <returns>A <see cref="Response"/> object representing the result of the operation.</returns>
        Response GetResponse(IVisualTreeContext context);
    }
}
