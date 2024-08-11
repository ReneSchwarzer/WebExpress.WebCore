using System;

namespace WebExpress.WebCore.WebModule
{
    public interface IModule : IDisposable
    {
        /// <summary>
        /// Instillation of the module. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        void Initialization(IModuleContext moduleContext);

        /// <summary>
        /// Called when the module starts working. The call is concurrent.
        /// </summary>
        void Run();
    }
}
