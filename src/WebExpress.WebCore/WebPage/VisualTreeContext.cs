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
        public IRequest Request => RenderContext?.Request;

        /// <summary>
        /// The uri of the request.
        /// </summary>
        public IUri Uri => RenderContext?.Request?.Uri;

        /// <summary>
        /// Return or sets the render context.
        /// </summary>
        public IRenderContext RenderContext { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="context">The context to copy.</param>
        public VisualTreeContext(IRenderContext context)
        {
            RenderContext = context;
        }
    }
}
