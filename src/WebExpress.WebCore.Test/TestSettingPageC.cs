using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:settingpageb.label")]
    [Segment("settingpagea", "webindex:homepage.label")]
    [ContextPath(null)]
    public sealed class TestSettingPageC : ISettingPage<VisualTree>
    {
        /// <summary>
        /// Returns or sets the setting page context.
        /// </summary>
        public ISettingPageContext PageContext { get; private set; }

        /// <summary>
        /// Initialization of the page. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="pageContext">The context of the setting page.</param>
        public TestSettingPageC(ISettingPageContext pageContext)
        {
            PageContext = pageContext;

            // test the injection
            if (pageContext == null)
            {
                throw new ArgumentNullException(nameof(pageContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Processing of the page.
        /// </summary>
        /// <param name="renderContext">The context for rendering the setting page.</param>
        /// <param name="visualTree">The visual tree to be rendered.</param>
        public void Process(IRenderContext renderContext, VisualTree visualTree)
        {
            // test the context
            if (renderContext == null)
            {
                throw new ArgumentNullException(nameof(renderContext), "Parameter cannot be null or empty.");
            }
        }
    }
}
