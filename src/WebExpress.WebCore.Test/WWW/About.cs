using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebScope;

namespace WebExpress.WebCore.Test.WWW
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:about.label")]
    public sealed class About : IPage, IScope
    {
        /// <summary>
        /// Gets or sets the title of the page.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the page context.
        /// </summary>
        public IPageContext PageContext { get; private set; }

        /// <summary>
        /// Initialization of the page. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="pageContext">The context of the page.</param>
        public About(IPageContext pageContext)
        {
            PageContext = pageContext;

            // test the injection
            if (pageContext is null)
            {
                throw new ArgumentNullException(nameof(pageContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Processing of the page.
        /// </summary>
        /// <param name="renderContext">The context for rendering the page.</param>
        /// <param name="visualTree">The visual tree to be rendered.</param>
        public void Process(IRenderContext renderContext, VisualTree visualTree)
        {
            // test the context
            if (renderContext is null)
            {
                throw new ArgumentNullException(nameof(renderContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
