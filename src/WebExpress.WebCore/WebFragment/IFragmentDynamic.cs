using System.Collections.Generic;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebFragment
{
    public interface IFragmentDynamic
    {
        /// <summary>
        /// Returns the context of the fragment..
        /// </summary>
        IFragmentContext Context { get; }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="page">The page where the fragment is active.</param>
        void Initialization(IFragmentContext context, IPage page);

        /// <summary>
        /// Creates fragments of a common type T.
        /// </summary>
        /// <returns>The created instances of the fragments.</returns>
        IEnumerable<T> Create<T>() where T : IComponent;
    }
}
