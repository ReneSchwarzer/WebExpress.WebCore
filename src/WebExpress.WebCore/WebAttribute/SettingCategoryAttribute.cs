using System;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to specify the category in which the settings page is associated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SettingCategoryAttribute<TCategory> : Attribute, ISettingGroupAttribute
        where TCategory : class, ISettingCategory
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SettingCategoryAttribute()
        {
        }
    }
}
