using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:homepage.label")]
    [Segment("pa1z", "webindex:homepage.label")]
    [ContextPath(null)]
    [Module<TestModuleA1>]
    public sealed class TestPageA1Z : Page<RenderContext>
    {
        /// <summary>
        /// Instillation of the page. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="pageContext">The context of the page.</param>
        private TestPageA1Z(IPageContext pageContext)
            : base(pageContext)
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
        public void Dispose()
        {
        }
    }
}
