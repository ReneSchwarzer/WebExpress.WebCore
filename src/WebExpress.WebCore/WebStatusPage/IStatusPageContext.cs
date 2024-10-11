using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebStatusPage
{
    /// <summary>
    /// Represents the context for a status page.
    /// </summary>
    public interface IStatusPageContext : IContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the associated application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Returns or sets the status id.
        /// </summary>
        string StatusId { get; }

        /// <summary>
        /// Returns or sets the status code.
        /// </summary>
        int StatusCode { get; }

        /// <summary>
        /// Returns or sets the status title.
        /// </summary>
        string StatusTitle { get; }

        /// <summary>
        /// Returns or sets the status icon.
        /// </summary>
        UriResource StatusIcon { get; }
    }
}
