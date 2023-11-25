using System;
using WebExpress.WebCore.WebModule;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifying a module.
    /// </summary>
    /// <typeparam name="T">The type of the module.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModuleAttribute<T> : Attribute, IResourceAttribute, IModuleAttribute where T : class, IModule
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ModuleAttribute()
        {

        }
    }
}
