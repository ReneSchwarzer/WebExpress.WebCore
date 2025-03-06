using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to specify the category in which the settings page is associated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SettingCategoryAttribute : Attribute, IEndpointAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="category">The category in which the settings page is associated.</param>
        public SettingCategoryAttribute(string category)
        {

        }
    }
}
