using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Marks an assembly as systemically relevant.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class SystemPluginAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SystemPluginAttribute()
        {

        }
    }
}
