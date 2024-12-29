using WebExpress.WebCore.WebEndpoint;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Defines the contract for a page resource.
    /// </summary>
    public interface IPage : IPage<VisualTree>
    {
    }

    /// <summary>
    /// Defines the contract for a page resource that can be rendered using a specific context.
    /// </summary>
    /// <typeparam name="TVisualTree">The type of the visual tree.</typeparam>
    public interface IPage<TVisualTree> : IEndpoint where TVisualTree : IVisualTree
    {
        /// <summary>
        /// Processing of the page.
        /// </summary>
        /// <param name="renderContext">The context for rendering the page.</param>
        /// <param name="visualTree">The visual tree to be rendered.</param>
        void Process(IRenderContext renderContext, TVisualTree visualTree);
    }
}
