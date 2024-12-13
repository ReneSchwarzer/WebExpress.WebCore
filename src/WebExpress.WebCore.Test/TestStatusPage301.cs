using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebStatusPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:homepage.label")]
    [StatusResponse<ResponseMovedPermanently>()]
    // [Icon("/webexpress/icon.png")] test empty icon
    public sealed class TestStatusPage301 : IStatusPage<VisualTree>
    {
        /// <summary>
        /// Initialization of the status page. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="statusPageContext">The context of the status page.</param>
        private TestStatusPage301(IStatusPageContext statusPageContext)
        {
            // test the injection
            if (statusPageContext == null)
            {
                throw new ArgumentNullException(nameof(statusPageContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Processing of the status page.
        /// </summary>
        /// <param name="renderContext">The context for rendering the status page.</param>
        /// <param name="visualTree">The visual tree to be rendered.</param>
        public void Process(IRenderContext renderContext, VisualTree visualTree)
        {
            // test the parameter
            if (renderContext == null)
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
