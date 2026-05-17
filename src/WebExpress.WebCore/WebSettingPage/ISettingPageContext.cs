using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Interface representing the context of a setting page.
    /// Provides access to plugin context, application context, conditions for activation, and caching behavior.
    /// </summary>
    public interface ISettingPageContext : IPageContext
    {
        /// <summary>
        /// Gets the setting category context to which the setting page belongs.
        /// </summary>
        ISettingCategoryContext SettingCategory { get; }

        /// <summary>
        /// Gets the group context to which the setting page belongs.
        /// </summary>
        ISettingGroupContext SettingGroup { get; }

        /// <summary>
        /// Gets the section to which the setting page belongs.
        /// </summary>
        SettingSection Section { get; }

        /// <summary>
        /// Gets a value indicating whether the page should be displayed or hidden.
        /// </summary>
        bool Hide { get; }
    }
}
