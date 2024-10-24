using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage.Model
{
    /// <summary>
    /// A page element that contains meta information about a page.
    /// </summary>
    internal class PageItem : IDisposable
    {
        private readonly IPageManager _pageManager;

        /// <summary>
        /// Returns or sets the resource title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns or sets the parent type.
        /// </summary>
        public Type ParentType { get; set; }

        /// <summary>
        /// Returns or sets the type of page.
        /// </summary>
        public Type PageClass { get; set; }

        /// <summary>
        /// Returns or sets the instance of the page, if the page is cached, otherwise null.
        /// </summary>
        public IPage Instance { get; set; }

        /// <summary>
        /// Returns the scope names that provides the resource. The scope name
        /// is a string with a name (e.g. global, admin), which can be used by elements to 
        /// determine whether content and how content should be displayed.
        /// </summary>
        public IReadOnlyList<string> Scopes { get; set; }

        /// <summary>
        /// Returns or sets the paths of the resource.
        /// </summary>
        public UriResource ContextPath { get; set; }

        /// <summary>
        /// Returns or sets the path segment.
        /// </summary>
        public IUriPathSegment PathSegment { get; internal set; }

        /// <summary>
        /// Returns or sets whether all subpaths should be taken into sitemap.
        /// </summary>
        public bool IncludeSubPaths { get; set; }

        /// <summary>
        /// Returns the conditions that must be met for the resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; set; }

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache { get; set; }

        /// <summary>
        /// Returns whether it is a optional resource.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Returns the log to write status messages to the console and to a log file.
        /// </summary>
        public ILog Log { get; internal set; }

        /// <summary>
        /// Returns the page context.
        /// </summary>
        public IPageContext PageContext { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageItem"/> class.
        /// </summary>
        /// <param name="pageManager">The page manager.</param>
        internal PageItem(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Convert the resource element to a string.
        /// </summary>
        /// <returns>The resource element in its string representation.</returns>
        public override string ToString()
        {
            return $"Page: '{PageContext?.EndpointId}'";
        }
    }
}
