using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebStatusPage
{
    public interface IStatusPageContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns or sets the status code.
        /// </summary>
        int Code { get; }

        /// <summary>
        /// Returns or sets the status title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Returns or sets the status icon.
        /// </summary>
        UriResource Icon { get; }
    }
}
