using System.Collections.Generic;

namespace WebExpress.WebCore.WebPlugin.Model
{
    /// <summary>
    /// Represents runtime metadata for a plugin.
    /// </summary>
    public sealed class PluginRuntimeInfo
    {
        /// <summary>
        /// Gets or sets the plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; set; }

        /// <summary>
        /// Gets or sets the plugin dependency ids.
        /// </summary>
        public IEnumerable<string> Dependencies { get; set; } = [];

        /// <summary>
        /// Gets or sets the plugin runtime state.
        /// </summary>
        public PluginRuntimeState State { get; set; }
    }
}
