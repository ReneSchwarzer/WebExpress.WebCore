using System;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebSettingPage.Model;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Interface representing the context of a setting page.
    /// Provides access to plugin context, application context, conditions for activation, and caching behavior.
    /// </summary>
    public class SettingPageContext : PageContext, ISettingPageContext
    {
        /// <summary>
        /// Returns the group to which the setting page belongs.
        /// </summary>
        public string Group { get; internal set; }

        /// <summary>
        /// Returns a value indicating whether the page should be displayed or hidden.
        /// </summary>
        public bool Hide { get; internal set; }

        /// <summary>
        /// Returns the icon.
        /// </summary>
        public string Icon { get; internal set; }

        /// <summary>
        /// Returns the setting category.
        /// </summary>
        public string Category { get; internal set; }

        /// <summary>  
        /// Returns the section of the setting page.  
        /// </summary>  
        public SettingSection Section { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the class with the specified parent type and context path.
        /// </summary>
        /// <param name="endpointManager">The endpoint manager responsible for managing endpoints.</param>
        /// <param name="parentType">The type of the parent resource.</param>
        /// <param name="contextPath">The context path of the resource.</param>
        /// <param name="pathSegment">The path segment of the resource.</param>
        public SettingPageContext(IEndpointManager endpointManager, Type parentType, UriResource contextPath, IUriPathSegment pathSegment)
            : base(endpointManager, parentType, contextPath, pathSegment)
        {
        }
    }
}
