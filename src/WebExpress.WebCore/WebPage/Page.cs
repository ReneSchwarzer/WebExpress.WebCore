namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// The prototype of a website.
    /// </summary>
    /// <typeparam name="TVisualTree">An implementation of the visualization tree.</typeparam>
    public abstract class Page<TVisualTree> : IPage<TVisualTree>
        where TVisualTree : IVisualTree, new()
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
        public Page()
        {
        }

        /// <summary>
        /// Redirect to another page.
        /// The function throws the RedirectException.
        /// </summary>
        /// <param name="uri">The uri to redirect to.</param>
        public virtual void Redirecting(string uri)
        {
            throw new RedirectException(uri?.ToString());
        }

        /// <summary>
        /// Processing of the page.
        /// </summary>
        /// <param name="renderContext">The context for rendering the page.</param>
        /// <param name="visualTree">The visual tree to be rendered.</param>
        public abstract void Process(IRenderContext renderContext, TVisualTree visualTree);

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();
    }
}
