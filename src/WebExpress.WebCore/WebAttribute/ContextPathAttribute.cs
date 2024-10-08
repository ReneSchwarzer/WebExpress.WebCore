using System;

namespace WebExpress.WebCore.WebAttribute
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ContextPathAttribute : Attribute, IApplicationAttribute, IModuleAttribute, IEndpointAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="contetxPath">The context path.</param>
        public ContextPathAttribute(string contetxPath)
        {

        }
    }
}
