using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Indicates that a status page ist default
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultAttribute : Attribute, IStatusPageAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DefaultAttribute()
        {

        }
    }
}
