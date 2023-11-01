using System;

namespace WebExpress.WebAttribute
{
    /// <summary>
    /// Marks a ressorce as optional. This becomes active when the option is specified in the application.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OptionalAttribute : Attribute, IResourceAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OptionalAttribute()
        {

        }
    }
}
