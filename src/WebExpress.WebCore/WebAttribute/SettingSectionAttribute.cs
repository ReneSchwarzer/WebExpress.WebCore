using System;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to specify the section where the settings page is listed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SettingSectionAttribute : Attribute, IEndpointAttribute, ISettingCategoryAttribute, ISettingGroupAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="section">The section where the settings page is listed.</param>
        public SettingSectionAttribute(SettingSection section)
        {

        }
    }
}
