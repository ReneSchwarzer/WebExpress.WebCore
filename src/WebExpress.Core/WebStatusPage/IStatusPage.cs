using WebExpress.Core.WebApplication;
using WebExpress.Core.WebMessage;
using WebExpress.Core.WebModule;
using WebExpress.Core.WebResource;
using WebExpress.Core.WebUri;

namespace WebExpress.Core.WebStatusPage
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
