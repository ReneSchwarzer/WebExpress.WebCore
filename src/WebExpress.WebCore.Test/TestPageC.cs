using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:pagec.label")]
    [Segment("pagec", "webindex:homepage.label")]
    [ContextPath(null)]
    public sealed class TestPageC : Page<RenderContext>
    {
        /// <summary>
        /// Initialization of the page. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="pageContext">The context of the page.</param>
        private TestPageC(IPageContext pageContext)
        {
            // test the injection
            if (pageContext == null)
            {
                throw new ArgumentNullException(nameof(pageContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Processing of the page.
        /// </summary>
        /// <param name="context">The context for rendering the page.</param>
        public override void Process(IRenderContext context)
        {
            // test the context
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Parameter cannot be null or empty.");
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
