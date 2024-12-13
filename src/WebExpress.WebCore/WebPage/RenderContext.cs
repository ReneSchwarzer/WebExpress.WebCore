using System.Globalization;
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
        public UriResource Uri => Request?.Uri;

        /// <summary>
        /// Returns the culture.
        /// </summary>
        public CultureInfo Culture => Request?.Culture;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RenderContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="pageContext">>The page context.</param>
        /// <param name="request">The request associated with the rendering context.</param>
        public RenderContext(IPageContext pageContext, Request request)
            : this()
        {
            PageContext = pageContext;
            Request = request;
        }

        /// <summary>
        /// Copy-Constructor
        /// </summary>
        /// <param name="context">The context to copy.</param>
        public RenderContext(RenderContext context)
            : this(context?.PageContext, context?.Request)
        {
        }
    }
}
