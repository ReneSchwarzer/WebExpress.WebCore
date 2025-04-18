using System;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to define a setting group in which the settings page is associated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SettingGroupAttribute<TGroup> : Attribute, IEndpointAttribute
        where TGroup : class, ISettingGroup
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SettingGroupAttribute()
        {
        }
    }
}
