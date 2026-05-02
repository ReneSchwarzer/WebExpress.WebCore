using System;
using WebExpress.WebCore.WebIcon;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to specify an icon for a plugin, application, or status page.
    /// </summary>
    /// <typeparam name="TIcon">The type of the icon, which must implement the <see cref="IIcon"/> interface.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WebIconAttribute<TIcon> : Attribute, ISettingPageAttribute, IPageAttribute, ISettingCategoryAttribute, ISettingGroupAttribute
        where TIcon : IIcon
    {
        /// <summary>
        /// Gets the icon theme used to display type icons.
        /// </summary>
        public TypeIconTheme Theme { get; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebIconAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class using the specified icon theme.
        /// </summary>
        /// <param name="theme">
        /// The icon theme applied to this attribute. The selected theme determines the visual appearance
        /// of the associated web icon.
        /// </param>

        public WebIconAttribute(TypeIconTheme theme)
        {
            Theme = theme;
        }
    }
}
