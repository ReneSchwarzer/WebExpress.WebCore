using System;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Connects policies with permissions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PolicyAttribute<T> : Attribute, IPolicyAttribute, IEndpointAttribute
        where T : class, IIdentityPolicy
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PolicyAttribute()
        {

        }
    }
}
