using System;
using WebExpress.WebCore.WebRestApi;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// The range in which the attribute is valid.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MethodAttribute : Attribute, IEndpointAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MethodAttribute(CrudMethod crudMethod)
        {

        }
    }
}
