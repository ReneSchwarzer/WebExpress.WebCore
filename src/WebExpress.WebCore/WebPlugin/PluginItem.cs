using System;
using System.Collections.Generic;
using System.Threading;

namespace WebExpress.WebCore.WebPlugin
{
    /// <summary>
    /// Represents a plugin entry.
    /// </summary>
    internal class PluginItem
    {
        /// <summary>
        /// The plugin load context for isolating and unloading the dependent libraries.
        /// </summary>
        public PluginLoadContext PluginLoadContext { get; internal set; }

        /// <summary>
        /// Returns the plugin class.
        /// </summary>
        public Type PluginClass { get; internal set; }

        /// <summary>
        /// The context associated with the plugin.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// The plugin.
        /// </summary>
        public IPlugin Plugin { get; internal set; }

        /// <summary>
        /// The dependencies of the plugin.
        /// </summary>
        public IEnumerable<string> Dependencies { get; internal set; } = new List<string>();

        /// <summary>
        /// Thread termination token.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
    }
}
