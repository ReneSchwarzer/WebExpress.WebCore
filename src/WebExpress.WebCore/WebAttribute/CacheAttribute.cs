using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Indicates that a page or component can be reused
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CacheAttribute : System.Attribute, IEndpointAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CacheAttribute()
        {

        }
    }
}
