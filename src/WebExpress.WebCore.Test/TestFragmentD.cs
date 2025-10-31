using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// Represents a test fragment that should never be displayed due to the condition.
    /// </summary>
    [Section<TestSectionA>()]
    [Scope<TestScopeD>]
    [Order(0)]
    [Condition<TestConditionAlwaysFalse>]
    public sealed class TestFragmentD : IFragment<TestRenderContext, TestVisualTree>
    {
        /// <summary>
        /// Returns or sets the id.
        /// </summary>
        public string Id => string.Empty;

        /// <summary>
        /// Initialization of the fragment. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="fragmentContext">The context of the fragment.</param>
        public TestFragmentD(IComponentHub componentHub, IFragmentContext fragmentContext)
        {
            // test the injection
            if (componentHub == null)
            {
                throw new ArgumentNullException(nameof(componentHub), "Parameter cannot be null or empty.");
            }

            // test the injection
            if (fragmentContext == null)
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
            return new HtmlText("TestFragmentD");
        }

        /// <summary>
        /// Disposes the resources used by the fragment.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
