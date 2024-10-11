using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents the interface of the context in which rendering occurs.
    /// </summary>
    public interface IRenderContext
    {
        /// <summary>
        /// Returns the application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Returns the request.
        /// </summary>
        Request Request { get; }

        /// <summary>
        /// Returns the scopes.
        /// </summary>
        IEnumerable<string> Scopes { get; }

        /// <summary>
        /// Returns the contents of a page.
        /// </summary>
        IVisualTree VisualTree { get; }
    }
}
