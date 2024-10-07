using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebStatusPage
{
    /// <summary>
    /// Interface of the status pages.
    /// </summary>
    public interface IStatusPage
    {
        /// <summary>
        /// Returns the resource context where the resource exists.
        /// </summary>
        IPageContext ResourceContext { get; }

        /// <summary>
        /// Returns or sets the status code.
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// Returns or sets the status title.
        /// </summary>
        string StatusTitle { get; set; }

        /// <summary>
        /// Returns or sets the status message.
        /// </summary>
        string StatusMessage { get; set; }

        /// <summary>
        /// Returns or sets the status icon.
        /// </summary>
        UriResource StatusIcon { get; set; }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="resourceContext">The context of the resource.</param>
        void Initialization(IPageContext resourceContext);

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        Response Process(Request request);
    }
}
