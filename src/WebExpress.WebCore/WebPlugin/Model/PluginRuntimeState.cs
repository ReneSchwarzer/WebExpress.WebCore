namespace WebExpress.WebCore.WebPlugin.Model
{
    /// <summary>
    /// Represents the runtime state of a plugin.
    /// </summary>
    public enum PluginRuntimeState
    {
        /// <summary>
        /// The plugin is loaded and active.
        /// </summary>
        Active,

        /// <summary>
        /// The plugin is known but waiting for one or more dependencies.
        /// </summary>
        WaitingForDependencies
    }
}
