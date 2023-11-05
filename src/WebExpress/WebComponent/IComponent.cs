using System.Collections.Generic;
using WebExpress.WebPlugin;

namespace WebExpress.WebComponent
{
    /// <summary>
    /// Interface of the manager classes.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Returns the reference to the context of the host.
        /// </summary>
        static IHttpServerContext HttpServerContext { get; }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The reference to the context of the host.</param>
        void Initialization(IHttpServerContext context);

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep);
    }
}
