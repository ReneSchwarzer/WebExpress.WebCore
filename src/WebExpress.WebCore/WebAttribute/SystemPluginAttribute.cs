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
        /// Constructor
        /// </summary>
        public SystemPluginAttribute()
        {

        }
    }
}
