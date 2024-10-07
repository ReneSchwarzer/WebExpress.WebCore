using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// The prototype of a website.
    /// </summary>
    /// <typeparam name="T">An implementation of the visualization tree.</typeparam>
    public abstract class Page<T> : Resource, IPage where T : RenderContext, new()
    {
        /// <summary>
        /// Returns or sets the page title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="pageContext">The context of the page.</param>
        public Page(IPageContext pageContext)
        {

        }

        /// <summary>
        /// Redirect to another page.
        /// The function throws the RedirectException.
        /// </summary>
        /// <param name="uri">The uri to redirect to.</param>
        public void Redirecting(string uri)
        {
            throw new RedirectException(uri?.ToString());
        }

        /// <summary>
        /// Processing of the page.
        /// </summary>
        /// <param name="context">The context for rendering the page.</param>
        public abstract void Process(IRenderContext context);
    }
}
