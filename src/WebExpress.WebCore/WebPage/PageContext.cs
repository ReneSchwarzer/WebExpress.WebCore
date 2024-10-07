using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPage.Model;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents the context of a page.
    /// </summary>
    public class PageContext : IPageContext
    {
        private readonly IPageManager _pageManager;
        private readonly PageItem _pageItem;

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; private set; }

        /// <summary>
        /// Returns the corresponding module context.
        /// </summary>
        public IModuleContext ModuleContext { get; private set; }

        /// <summary>
        /// Returns the scope names that provides the resource. The scope name
        /// is a string with a name (e.g. global, admin), which can be used by elements to 
        /// determine whether content and how content should be displayed.
        /// </summary>
        public IEnumerable<string> Scopes { get; internal set; }

        /// <summary>
        /// Returns the conditions that must be met for the resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; internal set; } = new List<ICondition>();

        /// <summary>
        /// Returns the endpoint id.
        /// </summary>
        public string EndpointId { get; internal set; }

        /// <summary>
        /// Returns the resource title.
        /// </summary>
        public string PageTitle { get; internal set; }

        /// <summary>
        /// Returns the parent or null if not used.
        /// </summary>
        public IEndpointContext ParentContext => _pageManager.Pages
            .Where(x => !string.IsNullOrWhiteSpace(_pageItem.ParentId))
            .Where(x => x.EndpointId.Equals(_pageItem.ParentId, StringComparison.OrdinalIgnoreCase))
            .Where(x => x.ModuleContext.ApplicationContext == ModuleContext.ApplicationContext)
            .FirstOrDefault();

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache { get; internal set; }

        /// <summary>
        /// Returns or sets whether all subpaths should be taken into sitemap.
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
                    return UriResource.Combine(ParentContext?.Uri, _pageItem.ContextPath);
                }

                return UriResource.Combine(ModuleContext.ContextPath, _pageItem.ContextPath);
            }
        }

        /// <summary>
        /// Returns the uri.
        /// </summary>
        public UriResource Uri => ContextPath.Append(_pageItem.PathSegment);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        /// <param name="pageManager">The resource manager.</param>
        /// <param name="pageItem">The page item or null.</param>
        internal PageContext(IModuleContext moduleContext, IPageManager pageManager, PageItem pageItem = null)
        {
            PluginContext = moduleContext?.PluginContext;
            ModuleContext = moduleContext;
            _pageManager = pageManager;
            _pageItem = pageItem;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{ModuleContext?.ApplicationContext?.ApplicationId}:{ModuleContext?.ModuleId}:{EndpointId}";
        }
    }
}
