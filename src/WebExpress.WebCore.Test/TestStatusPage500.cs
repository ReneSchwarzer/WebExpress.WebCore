using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebStatusPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:homepage.label")]
    [StatusResponse<ResponseInternalServerError>()]
    [Icon("/webexpress/icon.png")]
    public sealed class TestStatusPage500 : IStatusPage<VisualTree>
    {
        /// <summary>
        /// Returns or sets the status message.
        /// </summary>
        public string StatusMessage { get; private set; }

        /// <summary>
        /// Initialization of the status page. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="statusPageContext">The context of the status page.</param>
        /// <param name="message">The status message.</param>
        private TestStatusPage500(IStatusPageContext statusPageContext, StatusMessage message)
        {
            // test the injection
            if (statusPageContext == null)
            {
                throw new ArgumentNullException(nameof(statusPageContext), "Parameter cannot be null or empty.");
            }

            // test the injection
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Parameter cannot be null or empty.");
            }

            StatusMessage = message?.Message;
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

            visualTree.Content = new HtmlText(StatusMessage);
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
