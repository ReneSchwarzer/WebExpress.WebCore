using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Marks a ressorce as optional. This becomes active when the option is specified in the application.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OptionalAttribute : Attribute, IEndpointAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OptionalAttribute()
        {

        }
    }
}
