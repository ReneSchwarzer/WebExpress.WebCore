using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// Represents a test fragment.
    /// </summary>
    [Section<TestSectionA>()]
    [Scope<TestScopeA>]
    [Scope<TestPageB>]
    [Order(0)]
    public sealed class TestFragmentA : IFragment
    {
        /// <summary>
        /// Initialization of the fragment. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="fragmentContext">The context of the fragment.</param>
        /// <param name="id">The unique identifier for the fragment.</param>
        public TestFragmentA(IComponentHub componentHub, IFragmentContext fragmentContext, IComponentId id)
        {
            // test the injection
            if (componentHub == null)
            {
                throw new ArgumentNullException(nameof(componentHub), "Parameter componentHub cannot be null or empty.");
            }

            // test the injection
            if (fragmentContext == null)
            {
                throw new ArgumentNullException(nameof(fragmentContext), "Parameter fragmentContext cannot be null or empty.");
            }

            // test the injection
            if (string.IsNullOrWhiteSpace(id?.ToString()))
            {
                throw new ArgumentNullException(nameof(fragmentContext), "Parameter id cannot be null or empty.");
            }
        }

        /// <summary>
        /// Processes the fragments in the specified render context.
        /// </summary>
        /// <param name="renderContext">The context in which rendering occurs.</param>
        /// <returns>An HTML node representing the rendered fragments.</returns>
        public IHtmlNode Render(IRenderContext renderContext)
        {
            return new HtmlText("TestFragmentA");
        }

        /// <summary>
        /// Disposes the resources used by the fragment.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
