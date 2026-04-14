using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A custom render context for testing purposes.
    /// </summary>
    public class TestRenderContext : IRenderContext
    {
        /// <summary>
        /// Gets the endpoint associated with the rendering context.
        /// </summary>
        public IEndpoint Endpoint { get; protected set; }

        /// <summary>
        /// Gets the page context.
        /// </summary>
        public IPageContext PageContext { get; protected set; }

        /// <summary>
        /// Gets the request.
        /// </summary>
        public IRequest Request { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="endpoint">The endpoint associated with the rendering context.</param>
        /// <param name="pageContext">>The page context.</param>
        /// <param name="request">The request associated with the rendering context.</param>
        public TestRenderContext(IEndpoint endpoint, IPageContext pageContext, IRequest request)
        {
            Endpoint = endpoint;
            PageContext = pageContext;
            Request = request;
        }
    }
}
