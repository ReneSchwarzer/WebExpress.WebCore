using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents the interface of the context in which rendering occurs.
    /// </summary>
    public interface IRenderContext
    {
        /// <summary>
        /// Returns the endpoint associated with the rendering context.
        /// </summary>
        IEndpoint Endpoint { get; }

        /// <summary>
        /// Returns the page context.
        /// </summary>
        IPageContext PageContext { get; }

        /// <summary>
        /// The uri of the request.
        /// </summary>
        public IUri Uri => Request?.Uri;

        /// <summary>
        /// Returns the request.
        /// </summary>
        Request Request { get; }
    }
}
