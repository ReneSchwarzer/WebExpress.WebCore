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
        /// <param name="request">The request associated with the rendering context.</param>
        public VisualTreeContext(Request request)
        {
            Request = request;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="context">The context to copy./param>
        public VisualTreeContext(IRenderContext context)
            : this(context?.Request)
        {
        }
    }
}
