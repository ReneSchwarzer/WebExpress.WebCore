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
    }
}
