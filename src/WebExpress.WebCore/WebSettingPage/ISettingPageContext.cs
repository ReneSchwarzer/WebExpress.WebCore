using System.Collections.Generic;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Interface representing the context of a setting page.
    /// Provides access to plugin context, application context, conditions for activation, and caching behavior.
    /// </summary>
    public interface ISettingPageContext : IEndpointContext
    {
        /// <summary>
        /// Returns the setting page title.
        /// </summary>
        string SettingPageTitle { get; }

        /// <summary>
        /// Returns the scope names that provides the setting page. The scope name
        /// is a string with a name (e.g. global, admin), which can be used by elements to 
        /// determine whether content and how content should be displayed.
        /// </summary>
        IEnumerable<string> Scopes { get; }

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
