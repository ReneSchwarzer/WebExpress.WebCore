namespace WebExpress.WebCore.WebModule
{
    /// <summary>
    /// This represents an module.
    /// </summary>
    public abstract class Module : IModule
    {
        /// <summary>
        /// Returns the context of the module.
        /// </summary>
        public IModuleContext ModuleContext { get; private set; }

        /// <summary>
        /// Initialization of the module.
        /// </summary>
        /// <param name="moduleContext">The context.</param>
        public virtual void Initialization(IModuleContext moduleContext)
        {
            ModuleContext = moduleContext;
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
