using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebStatusPage
{
    public class StatusPageContext : IStatusPageContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns or sets the status code.
        /// </summary>
        public int Code { get; internal set; }

        /// <summary>
        /// Returns or sets the status title.
        /// </summary>
        public string Title { get; internal set; }

        /// <summary>
        /// Returns or sets the status icon.
        /// </summary>
        public UriResource Icon { get; internal set; }
    }
}
