using System;

namespace WebExpress.WebCore.WebModule
{
    public interface IModule : IDisposable
    {
        /// <summary>
        /// Returns the context of the module.
        /// </summary>
        IModuleContext ModuleContext { get; }

        /// <summary>
        /// Initialization of the module.
        /// </summary>
        /// <param name="moduleContext">The context.</param>
        void Initialization(IModuleContext moduleContext);

        /// <summary>
        /// Called when the module starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
