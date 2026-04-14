using System;
using System.Collections.Generic;
using System.Threading;

namespace WebExpress.WebCore.WebPlugin.Model
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
        /// Gets the plugin class.
        /// </summary>
        public Type PluginClass { get; internal set; }

        /// <summary>
        /// Gets the context associated with the plugin.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Gets the plugin.
        /// </summary>
        public IPlugin Plugin { get; internal set; }

        /// <summary>
        /// Gets the dependencies of the plugin.
        /// </summary>
        public IEnumerable<string> Dependencies { get; internal set; } = [];

        /// <summary>
        /// Gets the types of applications that the plugin supports.
        /// </summary>
        public IEnumerable<Type> ApplicationTypes { get; internal set; } = [];

        /// <summary>
        /// Gets the thread termination token.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
    }
}
