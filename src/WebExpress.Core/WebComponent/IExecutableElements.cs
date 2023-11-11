using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebComponent
{
    /// <summary>
    /// Indicates that a manager manages executable elements.
    /// </summary>
    public interface IExecutableElements
    {
        /// <summary>
        /// Boots the executable elements of a plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin containing the elements.</param>
        internal void Boot(IPluginContext pluginContext)
        {
            
        }

        /// <summary>
        /// Terminate the executable elements of a plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin containing the elements.</param>
        public void ShutDown(IPluginContext pluginContext)
        {

        }
    }
}
