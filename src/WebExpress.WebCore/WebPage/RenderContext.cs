using System.Globalization;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents the context in which rendering occurs, providing access to the page, request, culture, and visual tree.
    /// </summary>
    public class RenderContext : IRenderContext
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
        /// The uri of the request.
        /// </summary>
        public IUri Uri => Request?.Uri;

        /// <summary>
        /// Returns the culture.
        /// </summary>
        public CultureInfo Culture => Request?.Culture;

        /// <summary>
        /// Returns the endpoint associated with the rendering context.
        /// </summary>
        public IEndpoint Endpoint { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RenderContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="endpoint">The endpoint associated with the rendering context.</param>
        /// <param name="pageContext">The page context.</param>
        /// <param name="request">The request associated with the rendering context.</param>
        public RenderContext(IEndpoint endpoint, IPageContext pageContext, Request request)
        {
            Endpoint = endpoint;
            PageContext = pageContext;
            Request = request;
        }

        /// <summary>
        /// Copy-Constructor
        /// </summary>
        /// <param name="context">The context to copy.</param>
        public RenderContext(RenderContext context)
            : this(context.Endpoint, context?.PageContext, context?.Request)
        {
        }
    }
}
