using WebExpress.WebCore.WebSettingPage;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:settingpagea.label")]
    [Segment("settingpagea", "webindex:homepage.label")]
    [ContextPath(null)]
    public sealed class TestSettingPageA : ISettingPage<RenderContext>
    {
        /// <summary>
        /// Returns or sets the title of the setting page.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns or sets the setting page context.
        /// </summary>
        public ISettingPageContext PageContext { get; private set; }

        /// <summary>
        /// Initialization of the page. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="pageContext">The context of the setting page.</param>
        public TestSettingPageA(ISettingPageContext pageContext)
        {
            PageContext = pageContext;

            // test the injection
            if (pageContext == null)
            {
                throw new ArgumentNullException(nameof(pageContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Redirects to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to redirect to.</param>
        public void Redirecting(string uri)
        {

        }

        /// <summary>
        /// Processing of the page.
        /// </summary>
        /// <param name="context">The context for rendering the setting page.</param>
        public void Process(IRenderContext context)
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
