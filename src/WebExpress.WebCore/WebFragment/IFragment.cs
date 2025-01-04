using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebFragment
{
    /// <summary>
    /// Represents a fragment that is a part of a web component.
    /// </summary>
    public interface IFragment : IFragment<IRenderContext, IVisualTree>
    {
    }

    /// <summary>
    /// Represents a fragment that is a part of a web component.
    /// </summary>
    public interface IFragment<TRenderContext, TVisualTree> : IComponent, IFragmentBase
        where TRenderContext : IRenderContext
        where TVisualTree : IVisualTree
    {
        /// <summary>
        /// Convert the fragment to HTML.
        /// </summary>
        /// <param name="renderContext">The context in which the fragment is rendered.</param>
        /// <param name="visualTree">The visual tree used for rendering the fragment.</param>
        /// <returns>An HTML node representing the rendered fragments. Can be null if no nodes are present.</returns>
        IHtmlNode Render(TRenderContext renderContext, TVisualTree visualTree);
    }
}
