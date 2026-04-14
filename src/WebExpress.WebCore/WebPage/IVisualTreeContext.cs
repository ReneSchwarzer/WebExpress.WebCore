using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents the context of a visual tree.
    /// </summary>
    public interface IVisualTreeContext
    {
        /// <summary>
        /// Gets the request.
        /// </summary>
        IRequest Request { get; }

        /// <summary>
        /// Gets the uri of the request.
        /// </summary>
        IUri Uri { get; }

        /// <summary>
        /// Gets the render context.
        /// </summary>
        IRenderContext RenderContext { get; }
    }
}
