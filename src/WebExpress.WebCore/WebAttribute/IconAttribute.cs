using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to specify an icon for a plugin, application, or status page.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IconAttribute : Attribute, IPluginAttribute, IApplicationAttribute, IStatusPageAttribute, ISettingPageAttribute, ISettingCategoryAttribute, ISettingGroupAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="icon">The icon.</param>
        public IconAttribute(string icon)
        {

        }
    }
}
