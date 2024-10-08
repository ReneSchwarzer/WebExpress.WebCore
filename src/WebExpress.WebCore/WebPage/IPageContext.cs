using System.Collections.Generic;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Defines the context for a page, providing access to various related contexts and properties.
    /// </summary>
    public interface IPageContext : IEndpointContext
    {
        /// <summary>
        /// Returns the resource title.
        /// </summary>
        string PageTitle { get; }

        /// <summary>
        /// Returns the scope names that provides the resource. The scope name
        /// is a string with a name (e.g. global, admin), which can be used by elements to 
        /// determine whether content and how content should be displayed.
        /// </summary>
        IEnumerable<string> Scopes { get; }
    }
}
