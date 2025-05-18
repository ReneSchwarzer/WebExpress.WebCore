using WebExpress.WebCore.WebComponent;
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
    public interface IFragment<TRenderContext, TVisualTree> : IComponent, IFragmentBase, IWebUIElement<TRenderContext, TVisualTree>
        where TRenderContext : IRenderContext
        where TVisualTree : IVisualTree
    {
    }
}
