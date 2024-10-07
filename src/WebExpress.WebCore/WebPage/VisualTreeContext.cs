using System.Globalization;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents the context of a visual tree.
    /// </summary>
    public class VisualTreeContext : IVisualTreeContext
    {
        /// <summary>
        /// Returns the page where is rendered.
        /// </summary>
        public IPage Page { get; protected set; }

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
        /// Provides the context of the associated page.
        /// </summary>
        public IPageContext PageContext { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="page">The page where the rendering is taking place.</param>
        /// <param name="resourceContext">The context of the associated resource.</param>
        /// <param name="request">The request associated with the rendering context.</param>
        public VisualTreeContext(IPage page, IPageContext resourceContext, Request request)
        {
            Page = page;
            PageContext = resourceContext;
            Request = request;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="context">The context to copy./param>
        public VisualTreeContext(IRenderContext context)
            : this(context?.Page, context.PageContext, context?.Request)
        {
        }
    }
}
