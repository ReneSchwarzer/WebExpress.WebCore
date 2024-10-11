using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebStatusPage
{
    /// <summary>
    /// Represents the context for a status page.
    /// </summary>
    public class StatusPageContext : IStatusPageContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the associated application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns or sets the status id.
        /// </summary>
        public string StatusId { get; internal set; }

        /// <summary>
        /// Returns the status code.
        /// </summary>
        public int StatusCode { get; internal set; }

        /// <summary>
        /// Returns the status title.
        /// </summary>
        public string StatusTitle { get; internal set; }

        /// <summary>
        /// Returns the status icon.
        /// </summary>
        public UriResource StatusIcon { get; internal set; }
    }
}
