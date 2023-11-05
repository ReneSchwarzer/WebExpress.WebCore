using WebExpress.WebApplication;
using WebExpress.WebMessage;
using WebExpress.WebModule;
using WebExpress.WebResource;
using WebExpress.WebUri;

namespace WebExpress.WebStatusPage
{
    /// <summary>
    /// Statusseite
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
        /// Liefert oder setzt den Statuscode
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// Liefert oder setzt die Stausnachricht
        /// </summary>
        string StatusTitle { get; set; }

        /// <summary>
        /// Liefert oder setzt die Stausnachricht
        /// </summary>
        string StatusMessage { get; set; }

        /// <summary>
        /// Liefert oder setzt das Statusicon
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
