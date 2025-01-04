using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to define the context path for an application or endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ContextPathAttribute : Attribute, IApplicationAttribute, IEndpointAttribute
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
