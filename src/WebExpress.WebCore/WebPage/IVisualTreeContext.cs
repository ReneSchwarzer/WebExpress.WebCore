using System.Globalization;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents the context of a visual tree.
    /// </summary>
    public interface IVisualTreeContext
    {
        /// <summary>
        /// Returns the page where is rendered.
        /// </summary>
        IPage Page { get; }

        /// <summary>
        /// Returns the request.
        /// </summary>
        Request Request { get; }

        /// <summary>
        /// The uri of the request.
        /// </summary>
        UriResource Uri { get; }

        /// <summary>
        /// Returns the culture.
        /// </summary>
        CultureInfo Culture { get; }

        /// <summary>
        /// Provides the context of the associated page.
        /// </summary>
        IPageContext PageContext { get; }
    }
}
