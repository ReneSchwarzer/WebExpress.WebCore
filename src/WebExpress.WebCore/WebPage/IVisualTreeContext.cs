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
        /// Returns the request.
        /// </summary>
        IRequest Request { get; }

        /// <summary>
        /// The uri of the request.
        /// </summary>
        IUri Uri { get; }

        /// <summary>
        /// Return or sets the render context.
        /// </summary>
        IRenderContext RenderContext { get; }
    }
}
