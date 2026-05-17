using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.Test.WWW.Settings
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:settingpageb.label")]
    public sealed class TestSettingPageC : ISettingPage<VisualTree>
    {
        /// <summary>
        /// Gets or sets the setting page context.
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
            if (pageContext is null)
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
            if (renderContext is null)
            {
                throw new ArgumentNullException(nameof(renderContext), "Parameter cannot be null or empty.");
            }
        }
    }
}
