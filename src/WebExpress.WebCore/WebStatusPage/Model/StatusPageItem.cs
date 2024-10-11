using System;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebStatusPage.Model
{
    /// <summary>
    /// Represents a status page item.
    /// </summary>
    internal class StatusPageItem
    {
        /// <summary>
        /// Returns the status page id.
        /// </summary>
        public string StatusPageId { get; internal set; }

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the associated application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns status page context.
        /// </summary>
        public StatusPageContext StatusPageContext { get; internal set; }

        /// <summary>
        /// Returns the status code.
        /// </summary>
        public Type StatusResponse { get; internal set; }

        /// <summary>
        /// Returns the type of status page.
        /// </summary>
        public Type StatusPageClass { get; internal set; }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
