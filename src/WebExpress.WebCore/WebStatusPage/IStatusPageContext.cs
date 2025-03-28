using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;

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
        /// Returns the status page id.
        /// </summary>
        IComponentId StatusPageId { get; }

        /// <summary>
        /// Returns the status code.
        /// </summary>
        int StatusCode { get; }

        /// <summary>
        /// Returns the status title.
        /// </summary>
        string StatusTitle { get; }

        /// <summary>
        /// Returns the status icon.
        /// </summary>
        IRoute StatusIcon { get; }
    }
}
