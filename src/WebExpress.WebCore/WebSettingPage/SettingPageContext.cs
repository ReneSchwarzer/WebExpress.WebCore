using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebSettingPage.Model;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Interface representing the context of a setting page.
    /// Provides access to plugin context, application context, conditions for activation, and caching behavior.
    /// </summary>
    public class SettingPageContext : ISettingPageContext
    {
        private readonly IEndpointManager _endpointManager;
        private readonly Type _parentType;
        private readonly UriResource _contextPath;
        private readonly IUriPathSegment _pathSegment;

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
        /// Returns the setting page title.
        /// </summary>
        public string SettingPageTitle { get; internal set; }

        /// <summary>
        /// Returns the scope names that provides the setting page. The scope name
        /// is a string with a name (e.g. global, admin), which can be used by elements to 
        /// determine whether content and how content should be displayed.
        /// </summary>
        public IEnumerable<string> Scopes { get; internal set; } = [];

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
        public IEndpointContext ParentContext => _endpointManager.GetEndpoints(_parentType, ApplicationContext)
            .FirstOrDefault();

        /// <summary>  
        /// Returns a value indicating whether to include sub-paths.  
        /// </summary>  
        public bool IncludeSubPaths { get; internal set; }

        /// <summary>
        /// Returns the context path.
        /// </summary>
        public UriResource ContextPath
        {
            get
            {
                var parentContext = ParentContext;
                if (parentContext != null)
                {
                    return UriResource.Combine(ParentContext?.Uri, _contextPath);
                }

                return UriResource.Combine(ApplicationContext.ContextPath, _contextPath);
            }
        }

        /// <summary>  
        /// Returns the URI of the setting page.  
        /// </summary>  
        public UriResource Uri => ContextPath.Append(_pathSegment);

        /// <summary>
        /// Initializes a new instance of the class with the specified parent type and context path.
        /// </summary>
        /// <param name="endpointManager">The endpoint manager responsible for managing endpoints.</param>
        /// <param name="parentType">The type of the parent resource.</param>
        /// <param name="contextPath">The context path of the resource.</param>
        /// <param name="pathSegment">The path segment of the resource.</param>
        public SettingPageContext(IEndpointManager endpointManager, Type parentType, UriResource contextPath, IUriPathSegment pathSegment)
        {
            _endpointManager = endpointManager;
            _parentType = parentType;
            _contextPath = contextPath;
            _pathSegment = pathSegment;
        }
    }
}
