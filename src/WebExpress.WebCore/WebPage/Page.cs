namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// The prototype of a website.
    /// </summary>
    /// <typeparam name="T">An implementation of the visualization tree.</typeparam>
    public abstract class Page<T> : IPage<T> where T : IRenderContext, new()
    {
        /// <summary>
        /// Returns or sets the page title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns the page context.
        /// </summary>
        public IPageContext PageContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="pageContext">The context of the page.</param>
        public Page()
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

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();
    }
}
