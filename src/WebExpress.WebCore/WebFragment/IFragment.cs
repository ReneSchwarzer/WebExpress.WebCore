using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebFragment
{
    /// <summary>
    /// Represents a fragment that is a part of a web component.
    /// </summary>
    public interface IFragment : IComponent
    {
        /// <summary>
        /// Processes the fragments in the specified render context.
        /// </summary>
        /// <param name="renderContext">The context in which rendering occurs.</param>
        void Process(IRenderContext renderContext);
    }
}
