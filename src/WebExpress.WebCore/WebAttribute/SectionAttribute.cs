using System;
using WebExpress.WebCore.WebSection;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies a section type for a class, allowing the class to be associated with a 
    /// specific configuration or settings section.
    /// </summary>
    /// <typeparam name="TSection">
    /// The type of the section associated with the class. Must be a reference type that 
    /// implements <see cref="ISection"/>.
    /// </typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SectionAttribute<TSection> : Attribute, IFragmentAttribute, ISettingCategoryAttribute, ISettingGroupAttribute where TSection : class, ISection
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SectionAttribute()
        {
        }
    }
}
