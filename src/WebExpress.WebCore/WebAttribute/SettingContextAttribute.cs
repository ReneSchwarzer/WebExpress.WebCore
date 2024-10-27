using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to specify the context in which the settings page is associated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingContextAttribute : Attribute, IEndpointAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="context">The context in which the settings page is associated.</param>
        public SettingContextAttribute(string context)
        {

        }
    }
}
