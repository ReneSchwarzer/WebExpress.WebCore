using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebFragment
{
    /// <summary>
    /// Represents a web UI element that is both a fragment and a part of the visual tree,  designed to render within a
    /// specified control context.
    /// </summary>
    /// <typeparam name="TRenderControlContext">The type of the rendering control context, which must implement <see cref="IRenderContext"/>.</typeparam>
    /// <typeparam name="TVisualTree">The type of the visual tree structure, which must implement <see cref="IVisualTree"/>.</typeparam>
    public interface IFragmentWebUIElement<TRenderControlContext, TVisualTree> : IFragment<TRenderControlContext, TVisualTree>, IWebUIElement<TRenderControlContext, TVisualTree>
        where TRenderControlContext : IRenderContext
        where TVisualTree : IVisualTree
    {
    }
}
