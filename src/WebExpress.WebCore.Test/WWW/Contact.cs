using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.Test.WWW
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:contact.label")]
    public sealed class Contact : Page<TestVisualTree>
    {
        /// <summary>
        /// Initialization of the page. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="pageContext">The context of the page.</param>
        private Contact(IPageContext pageContext)
        {
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
        public override void Process(IRenderContext renderContext, TestVisualTree visualTree)
        {
            // test the context
            if (renderContext is null)
            {
                throw new ArgumentNullException(nameof(renderContext), "Parameter cannot be null or empty.");
            }

            // test the visualTree
            if (visualTree is null)
            {
                throw new ArgumentNullException(nameof(visualTree), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public override void Dispose()
        {
        }
    }
}
