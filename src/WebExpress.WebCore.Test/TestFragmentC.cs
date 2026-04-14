using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// Represents a test fragment.
    /// </summary>
    [Section<TestSectionA>()]
    [Scope<TestScopeC>]
    [Order(0)]
    public sealed class TestFragmentC : IFragment<TestRenderContext, TestVisualTree>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id => string.Empty;

        /// <summary>
        /// Initialization of the fragment. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="fragmentContext">The context of the fragment.</param>
        public TestFragmentC(IComponentHub componentHub, IFragmentContext fragmentContext)
        {
            // test the injection
            if (componentHub is null)
            {
                throw new ArgumentNullException(nameof(componentHub), "Parameter cannot be null or empty.");
            }

            // test the injection
            if (fragmentContext is null)
            {
                throw new ArgumentNullException(nameof(fragmentContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Processes the fragments in the specified render context.
        /// </summary>
        /// <param name="renderContext">The context in which rendering occurs.</param>
        /// <param name="visualTree">The visual tree used for rendering the fragment.</param>
        /// <returns>An HTML node representing the rendered fragments.</returns>
        public IHtmlNode Render(TestRenderContext renderContext, TestVisualTree visualTree)
        {
            return new HtmlText("TestFragmentC");
        }

        /// <summary>
        /// Disposes the resources used by the fragment.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
