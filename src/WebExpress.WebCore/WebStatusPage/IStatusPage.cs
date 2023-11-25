using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebResource;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebStatusPage
{
    /// <summary>
    /// Interface of the status pages.
    /// </summary>
    public interface IStatusPage
    {
        /// <summary>
        /// Returns the resource Id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Returns the context of the application.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Returns the context of the module.
        /// </summary>
        IModuleContext ModuleContext { get; }

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
        void Initialization(IResourceContext resourceContext);

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        Response Process(Request request);
    }
}
