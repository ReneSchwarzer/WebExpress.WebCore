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

        /// <summary>
        /// Redirect to another page.
        /// The function throws the RedirectException.
        /// </summary>
        /// <param name="uri">The uri to redirect to.</param>
        void Redirecting(string uri);
    }

    /// <summary>
    /// Defines the contract for a page resource that can be rendered using a specific context.
    /// </summary>
    /// <typeparam name="T">The type of the render context.</typeparam>
    public interface IPage<T> : IPage where T : IRenderContext
    {
    }
}
