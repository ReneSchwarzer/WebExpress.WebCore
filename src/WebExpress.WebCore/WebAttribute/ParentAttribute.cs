using System;
using WebExpress.WebCore.WebEndpoint;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to specify the parent endpoint for a given endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParentAttribute<TEndpoint> : Attribute, IEndpointAttribute
        where TEndpoint : class, IEndpoint
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ParentAttribute()
        {

        }
    }
}
