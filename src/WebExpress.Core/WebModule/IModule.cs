using System;

namespace WebExpress.Core.WebModule
{
    public interface IModule : IDisposable
    {
        /// <summary>
        /// Initialization of the module.
        /// </summary>
        /// <param name="context">The context.</param>
        void Initialization(IModuleContext context);

        /// <summary>
        /// Called when the module starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
