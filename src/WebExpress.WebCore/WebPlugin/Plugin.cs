namespace WebExpress.WebCore.WebPlugin
{
    /// <summary>
    /// This represents an plugin.
    /// </summary>
    public abstract class Plugin : IPlugin
    {
        /// <summary>
        /// Returns the context of the plugin.
        /// </summary>
        public IPluginContext PluginContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Plugin()
        {
        }

        /// <summary>
        /// Initialization of the application. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="pluginContext">The context that applies to the execution of the application</param>
        public virtual void Initialization(IPluginContext pluginContext)
        {
            PluginContext = pluginContext;
        }

        /// <summary>
        /// Called when the application starts working. The call is concurrent. 
        /// </summary>
        public virtual void Run()
        {
        }

        /// <summary>
        /// Release unmanaged resources that have been reserved during use.
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
