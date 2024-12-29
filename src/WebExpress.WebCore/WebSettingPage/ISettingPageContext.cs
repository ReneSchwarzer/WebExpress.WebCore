using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Interface representing the context of a setting page.
    /// Provides access to plugin context, application context, conditions for activation, and caching behavior.
    /// </summary>
    public interface ISettingPageContext : IPageContext
    {
        /// <summary>
        /// Returns the group to which the setting page belongs.
        /// </summary>
        string Group { get; }

        /// <summary>
        /// Returns the section to which the setting page belongs.
        /// </summary>
        SettingSection Section { get; }

        /// <summary>
        /// Returns a value indicating whether the page should be displayed or hidden.
        /// </summary>
        bool Hide { get; }

        /// <summary>
        /// Returns or sets the icon.
        /// </summary>
        string Icon { get; }
    }
}
