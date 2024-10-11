using System.Collections.Generic;
using System.Globalization;
using WebExpress.WebCore.WebApplication;
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
        /// Returns the application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; protected set; }

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
        /// Returns the scopes.
        /// </summary>
        public IEnumerable<string> Scopes { get; }

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
        /// <param name="applicationContext">>The application context.</param>
        /// <param name="request">The request associated with the rendering context.</param>
        /// <param name="scopes">The scopes associated with the rendering context.</param>
        public RenderContext(IApplicationContext applicationContext, Request request, IEnumerable<string> scopes)
            : this()
        {
            ApplicationContext = applicationContext;
            Request = request;
            Scopes = scopes;
        }

        /// <summary>
        /// Copy-Constructor
        /// </summary>
        /// <param name="context">The context to copy.</param>
        public RenderContext(RenderContext context)
            : this(context?.ApplicationContext, context?.Request, context?.Scopes)
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
