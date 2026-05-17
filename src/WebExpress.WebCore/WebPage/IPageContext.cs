using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebIcon;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Defines the context for a page, providing access to various related contexts and properties.
    /// </summary>
    public interface IPageContext : WebEndpoint.IEndpointContext
    {
        /// <summary>
        /// Gets the page title.
        /// </summary>
        string PageTitle { get; }

        /// <summary>
        /// Gets the page icon.
        /// </summary>
        IIcon PageIcon { get; }

        /// <summary>
        /// Gets the scope names that provides the page. The scope name
        /// is a string with a name (e.g. global, admin), which can be used by elements to 
        /// determine whether content and how content should be displayed.
        /// </summary>
        IEnumerable<Type> Scopes { get; }

        /// <summary>
        /// Gets the collection of domain types associated with the decorated element.
        /// Domains represent logical application areas such as workspaces, modules
        /// or functional segments and can be used for routing, filtering or contextual grouping.
        /// </summary>
        IEnumerable<Type> Domains { get; }
    }
}
