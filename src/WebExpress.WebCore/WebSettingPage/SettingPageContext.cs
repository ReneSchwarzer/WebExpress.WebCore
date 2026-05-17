using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Interface representing the context of a setting page.
    /// Provides access to plugin context, application context, conditions for activation, and caching behavior.
    /// </summary>
    public class SettingPageContext : PageContext, ISettingPageContext
    {
        /// <summary>
        /// Gets the setting category context to which the setting page belongs.
        /// </summary>
        public ISettingCategoryContext SettingCategory => SettingGroup?.SettingCategory;

        /// <summary>
        /// Gets the group context to which the setting page belongs.
        /// </summary>
        public ISettingGroupContext SettingGroup { get; internal set; }

        /// <summary>  
        /// Gets the section of the setting page.  
        /// </summary>  
        public SettingSection Section { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the page should be displayed or hidden.
        /// </summary>
        public bool Hide { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the class with the specified parent type and context path.
        /// </summary>
        public SettingPageContext()
            : base()
        {
        }
    }
}
