using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Indicates that a page or component can be reused
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CacheAttribute : Attribute, IEndpointAttribute, IIncludeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CacheAttribute()
        {

        }
    }
}
