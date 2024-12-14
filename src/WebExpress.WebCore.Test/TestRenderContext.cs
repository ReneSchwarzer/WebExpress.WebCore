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
        /// Returns the page context.
        /// </summary>
        public IPageContext PageContext { get; protected set; }

        /// <summary>
        /// Returns the request.
        /// </summary>
        public Request Request { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="pageContext">>The page context.</param>
        /// <param name="request">The request associated with the rendering context.</param>
        public TestRenderContext(IPageContext pageContext, Request request)
        {
        }
    }
}
