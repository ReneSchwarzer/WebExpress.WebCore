using System;
using WebExpress.WebCore.WebSection;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to identify a section.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SectionAttribute<T> : Attribute, IFragmentAttribute, ISettingCategoryAttribute, ISettingGroupAttribute where T : class, ISection
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SectionAttribute()
        {
        }
    }
}
