namespace WebExpress.WebCore.WebApplication
{
    /// <summary>
    /// This represents an application.
    /// </summary>
    public abstract class Application : IApplication
    {
        /// <summary>
        /// Returns the context of the application.
        /// </summary>
        public IApplicationContext ApplicationContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application()
        {
        }

        /// <summary>
        /// Initialization of the application. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="applicationContext">The context that applies to the execution of the application</param>
        public virtual void Initialization(IApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
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
