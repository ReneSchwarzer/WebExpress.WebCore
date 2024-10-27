using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebSettingPage.Model;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.SettingPage
{
    /// <summary>
    /// Interface representing the context of a setting page.
    /// Provides access to plugin context, application context, conditions for activation, and caching behavior.
    /// </summary>
    public class SettingPageContext : ISettingPageContext
    {
        /// <summary>
        /// Returns the context of the associated plugin.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns the unique identifier for the setting page.
        /// </summary>
        public string EndpointId { get; internal set; }

        /// <summary>
        /// Returns the group to which the setting page belongs.
        /// </summary>
        public string Group { get; internal set; }

        /// <summary>
        /// Returns the conditions that must be met for the component to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; internal set; }

        /// <summary>
        /// Returns a value indicating whether the component is created once and reused on each execution.
        /// </summary>
        public bool Cache { get; internal set; }

        /// <summary>
        /// Returns a value indicating whether the page should be displayed or hidden.
        /// </summary>
        public bool Hide { get; internal set; }

        /// <summary>
        /// Returns the icon.
        /// </summary>
        public string Icon { get; internal set; }

        /// <summary>
        /// Returns the setting context.
        /// </summary>
        public string Context { get; internal set; }

        /// <summary>  
        /// Returns the section of the setting page.  
        /// </summary>  
        public SettingSection Section { get; internal set; }

        /// <summary>  
        /// Returns the parent context of the endpoint.  
        /// </summary>  
        public IEndpointContext ParentContext { get; internal set; }

        /// <summary>  
        /// Returns a value indicating whether to include sub-paths.  
        /// </summary>  
        public bool IncludeSubPaths { get; internal set; }

        /// <summary>  
        /// Returns the context path of the URI resource.  
        /// </summary>  
        public UriResource ContextPath { get; internal set; }

        /// <summary>  
        /// Returns the URI of the setting page.  
        /// </summary>  
        public UriResource Uri { get; internal set; }

    }
}
