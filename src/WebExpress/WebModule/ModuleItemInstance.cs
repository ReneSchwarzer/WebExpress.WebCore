using System.Threading;

namespace WebExpress.WebModule
{
    public class ModuleItemInstance
    {
        /// <summary>
        /// Returns the module context.
        /// </summary>
        public IModuleContext ModuleContext { get; internal set; }

        /// <summary>
        /// Returns the module instance or null if not already created.
        /// </summary>
        public IModule ModuleInstance { get; internal set; }

        /// <summary>
        /// Returns the cancel token or null if not already created.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
    }
}
