using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents the interface of the context in which rendering occurs.
    /// </summary>
    public interface IRenderContext
    {
        /// <summary>
        /// Returns the page context.
        /// </summary>
        IPageContext PageContext { get; }

        /// <summary>
        /// Returns the request.
        /// </summary>
        Request Request { get; }
    }
}
