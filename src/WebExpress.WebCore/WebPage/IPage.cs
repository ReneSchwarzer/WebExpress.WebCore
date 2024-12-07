using WebExpress.WebCore.WebEndpoint;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Defines the contract for a page resource.
    /// </summary>
    public interface IPage : IEndpoint
    {
        /// <summary>
        /// Processing of the page.
        /// </summary>
        /// <param name="context">The context for rendering the page.</param>
        void Process(IRenderContext context);
    }

    /// <summary>
    /// Defines the contract for a page resource that can be rendered using a specific context.
    /// </summary>
    /// <typeparam name="T">The type of the render context.</typeparam>
    public interface IPage<T> : IPage where T : IRenderContext
    {
    }
}
