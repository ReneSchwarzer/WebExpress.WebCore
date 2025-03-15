using System;
using WebExpress.WebCore.WebIcon;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to specify an icon for a plugin, application, or status page.
    /// </summary>
    /// <typeparam name="TIcon">The type of the icon, which must implement the <see cref="IIcon"/> interface.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WebIconAttribute<TIcon> : Attribute, ISettingPageAttribute, ISettingCategoryAttribute, ISettingGroupAttribute
        where TIcon : IIcon
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebIconAttribute()
        {

        }
    }
}
