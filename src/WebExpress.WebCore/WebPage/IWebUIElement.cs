using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents a UI element that can be rendered to an HTML representation.
    /// </summary>
    /// <typeparam name="TRenderControlContext">The type of the rendering context used during the rendering process.</typeparam>
    /// <typeparam name="TVisualTree">The type of the visual tree that represents the structure of the page.</typeparam>
    public interface IWebUIElement<TRenderControlContext, TVisualTree>
        where TRenderControlContext : IRenderContext
        where TVisualTree : IVisualTree
    {
        /// <summary>
        /// Converts the control to an HTML representation.
        /// </summary>
        /// <param name="renderContext">The context in which the ui element is rendered.</param>
        /// <param name="visualTree">The visual tree representing the structure of the page.</param>
        /// <returns>An HTML node representing the rendered ui element.</returns>
        IHtmlNode Render(TRenderControlContext renderContext, TVisualTree visualTree);
    }
}
