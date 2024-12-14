using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebFragment
{
    /// <summary>
    /// Represents a fragment that is a part of a web component.
    /// </summary>
    public interface IFragment : IFragment<IRenderContext>
    {
    }

    /// <summary>
    /// Represents a fragment that is a part of a web component.
    /// </summary>
    public interface IFragment<T> : IComponent, IFragmentBase where T : IRenderContext
    {
        /// <summary>
        /// Convert the fragment to HTML.
        /// </summary>
        /// <param name="renderContext">The context in which the fragment is rendered.</param>
        /// <returns>An HTML node representing the rendered fragments. Can be null if no nodes are present.</returns>
        IHtmlNode Render(T renderContext);
    }
}
