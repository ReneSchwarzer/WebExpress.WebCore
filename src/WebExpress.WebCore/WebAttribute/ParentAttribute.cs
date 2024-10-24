using System;
using WebExpress.WebCore.WebEndpoint;

namespace WebExpress.WebCore.WebAttribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParentAttribute<T> : Attribute, IEndpointAttribute where T : class, IEndpoint
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ParentAttribute()
        {

        }
    }
}
