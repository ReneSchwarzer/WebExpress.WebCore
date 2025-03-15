using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebIcon;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    /// <summary>
    /// Represents an item on the setting page.
    /// </summary>
    public class SettingPageItem : IDisposable
    {
        private readonly IEndpointManager _endpointManager;
        private SettingPageContext _settingPageContext;

        /// <summary>
        /// Returns the endpoint id.
        /// </summary>
        public IComponentId EndpointId { get; internal set; }

        /// <summary>
        /// Returns the context of the associated plugin.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns the setting page context.
        /// </summary>
        public ISettingPageContext SettingPageContext
        {
            get
            {
                _settingPageContext ??= new SettingPageContext()
                {
                    PageTitle = PageTitle,
                    EndpointId = EndpointId,
                    PluginContext = PluginContext,
                    ApplicationContext = ApplicationContext,
                    Cache = Cache,
                    Scopes = Scopes,
                    Conditions = Conditions,
                    IncludeSubPaths = IncludeSubPaths,
                    Attributes = Attributes,
                    SettingGroup = SettingGroup,
                    Icon = Icon,
                };

                var parentContext = _endpointManager.GetEndpoints(ParentType, ApplicationContext)
                    .FirstOrDefault();

                var contextPath = UriResource.Combine
                (
                    parentContext?.Uri ?? ApplicationContext.ContextPath, ContextPath
                );

                _settingPageContext.ParentContext = parentContext;
                _settingPageContext.ContextPath = contextPath;
                _settingPageContext.Uri = contextPath.Append(PathSegment);

                return _settingPageContext;
            }
        }

        /// <summary>
        /// Returns the class type of the setting page.
        /// </summary>
        public Type SettingPageClass { get; internal set; }

        /// <summary>
        /// Returns the instance of the setting page, if the page is cached, otherwise null.
        /// </summary>
        public IEndpoint Instance { get; internal set; }

        /// <summary>
        /// Returns or sets the parent type.
        /// </summary>
        public Type ParentType { get; set; }

        /// <summary>
        /// Returns or sets the paths of the resource.
        /// </summary>
        public UriResource ContextPath { get; set; }

        /// <summary>
        /// Returns or sets the path segment.
        /// </summary>
        public IUriPathSegment PathSegment { get; internal set; }

        /// <summary>
        /// Returns the group type.
        /// </summary>
        public Type SettingGroupType { get; internal set; }

        /// <summary>
        /// Returns the section.
        /// </summary>
        public SettingSection Section { get; internal set; }

        /// <summary>
        /// Returns a value indicating whether the component is created once and reused on each execution.
        /// </summary>
        public bool Cache { get; internal set; }

        /// <summary>
        /// Returns the icon.
        /// </summary>
        public IIcon Icon { get; internal set; }

        /// <summary>
        /// Returns the setting page title.
        /// </summary>
        public string PageTitle { get; internal set; }

        /// <summary>
        /// Returns a value indicating whether the page should be displayed or hidden.
        /// </summary>
        public bool Hide { get; internal set; }

        /// <summary>
        /// Returns or sets whether all subpaths should be taken into sitemap.
        /// </summary>
        public bool IncludeSubPaths { get; internal set; }

        /// <summary>
        /// Returns the attributes associated with the page.
        /// </summary>
        public IEnumerable<Type> Attributes { get; internal set; }

        /// <summary>
        /// Returns the scope names that provides the resource. The scope name
        /// is a string with a name (e.g. global, admin), which can be used by elements to 
        /// determine whether content and how content should be displayed.
        /// </summary>
        public IEnumerable<Type> Scopes { get; internal set; } = [];

        /// <summary>
        /// Returns the conditions that must be met for the resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; internal set; } = [];

        /// <summary>
        /// Returns the group context to which the setting page belongs.
        /// </summary>
        public ISettingGroupContext SettingGroup { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="endpointManager">The endpoint manager responsible for managing endpoints.</param>
        internal SettingPageItem(IEndpointManager endpointManager)
        {
            _endpointManager = endpointManager;
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
