using System.Globalization;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents the interface of the context in which rendering occurs, providing access to the page, request, culture, and visual tree.
    /// </summary>
    public interface IRenderContext
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
        /// Provides the context of the associated resource.
        /// </summary>
        IPageContext PageContext { get; }

        /// <summary>
        /// Returns the contents of a page.
        /// </summary>
        IVisualTree VisualTree { get; }
    }
}
