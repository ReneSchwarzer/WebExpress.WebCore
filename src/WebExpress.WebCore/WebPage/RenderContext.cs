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
        /// Returns the contents of a page.
        /// </summary>
        public IVisualTree VisualTree { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RenderContext()
        {
            VisualTree = CreateVisualTree();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="page">The page where the rendering is taking place.</param>
        /// <param name="pageContext">The context of the associated page.</param>
        /// <param name="request">The request associated with the rendering context.</param>
        public RenderContext(IPage page, IPageContext pageContext, Request request)
            : this()
        {
            Page = page;
            PageContext = pageContext;
            Request = request;
        }

        /// <summary>
        /// Copy-Constructor
        /// </summary>
        /// <param name="context">The context to copy./param>
        public RenderContext(RenderContext context)
            : this(context?.Page, context.PageContext, context?.Request)
        {
        }

        /// <summary>
        /// Creates the visual tree representing the contents of a page.
        /// </summary>
        /// <returns>A new instance of the <see cref="IVisualTree"/> interface.</returns>
        protected virtual IVisualTree CreateVisualTree()
        {
            return new VisualTree();
        }
    }
}
