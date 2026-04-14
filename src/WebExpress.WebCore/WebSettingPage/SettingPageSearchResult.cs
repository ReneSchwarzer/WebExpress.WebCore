using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Represents the result of a search on the settings page.
    /// </summary>
    public class SettingPageSearchResult
    {
        /// <summary>
        /// Gets the setting context.
        /// </summary>
        public string Context { get; internal set; }

        /// <summary>
        /// Gets the section.
        /// </summary>
        public SettingSection Section { get; internal set; }

        /// <summary>
        /// Gets the group.
        /// </summary>
        public string Group { get; internal set; }

        /// <summary>
        /// Gets a list of all currently existing setting contexts that can be accessed through the settings page.
        /// </summary>
        public SettingPageItem Item { get; internal set; }
    }
}
