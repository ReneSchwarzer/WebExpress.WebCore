using System;

namespace WebExpress.WebCore.WebAttribute
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ContextPathAttribute : Attribute, IApplicationAttribute, IModuleAttribute, IResourceAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contetxPath">The context path.</param>
        public ContextPathAttribute(string contetxPath)
        {

        }
    }
}
