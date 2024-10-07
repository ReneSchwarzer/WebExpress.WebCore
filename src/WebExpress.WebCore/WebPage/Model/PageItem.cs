using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebModule;
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
        /// An event that fires when an page is added.
        /// </summary>
        public event EventHandler<IPageContext> AddPage;

        /// <summary>
        /// An event that fires when an page is removed.
        /// </summary>
        public event EventHandler<IPageContext> RemovePage;

        /// <summary>
        /// Returns or sets the page id.
        /// </summary>
        public string PageId { get; set; }

        /// <summary>
        /// Returns or sets the resource title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns or sets the parent id.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Returns or sets the type of page.
        /// </summary>
        public Type PageClass { get; set; }

        /// <summary>
        /// Returns or sets the instance of the page, if the page is cached, otherwise null.
        /// </summary>
        public IPage Instance { get; set; }

        /// <summary>
        /// Returns or sets the module id.
        /// </summary>
        public string ModuleId { get; set; }

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
        public ICollection<ICondition> Conditions { get; set; }

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
        /// Returns the directory where the module instances are listed.
        /// </summary>
        private IDictionary<IModuleContext, IPageContext> Dictionary { get; }
            = new Dictionary<IModuleContext, IPageContext>();

        /// <summary>
        /// Returns the associated module contexts.
        /// </summary>
        public IEnumerable<IModuleContext> ModuleContexts => Dictionary.Keys;

        /// <summary>
        /// Returns the page contexts.
        /// </summary>
        public IEnumerable<IPageContext> PageContexts => Dictionary.Values;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageItem"/> class.
        /// </summary>
        /// <param name="pageManager">The page manager.</param>
        internal PageItem(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        /// <summary>
        /// Adds an module assignment
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        public void AddModule(IModuleContext moduleContext)
        {
            // only if no instance has been created yet
            if (Dictionary.ContainsKey(moduleContext))
            {
                Log.Warning(message: I18N.Translate("webexpress:pagemanager.addresource.duplicate", PageId, moduleContext.ModuleId));

                return;
            }

            // create context
            var pageContext = new PageContext(moduleContext, _pageManager, this)
            {
                Scopes = Scopes,
                Conditions = Conditions,
                EndpointId = PageId,
                PageTitle = Title,
                Cache = Cache,
                IncludeSubPaths = IncludeSubPaths
            };

            if
            (
                !Optional ||
                moduleContext.ApplicationContext.Options.Contains($"{ModuleId.ToLower()}.{PageId.ToLower()}") ||
                moduleContext.ApplicationContext.Options.Contains($"{ModuleId.ToLower()}.*") ||
                moduleContext.ApplicationContext.Options.Where(x => Regex.Match($"{ModuleId.ToLower()}.{PageId.ToLower()}", x).Success).Any()
            )
            {
                Dictionary.Add(moduleContext, pageContext);
                OnAddPage(pageContext);
            }
        }

        /// <summary>
        /// Remove an module assignment
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        public void DetachModule(IModuleContext moduleContext)
        {
            // not an assignment has been created yet
            if (!Dictionary.ContainsKey(moduleContext))
            {
                return;
            }

            foreach (var resourceContext in Dictionary.Values)
            {
                OnRemovePage(resourceContext);
            }

            Dictionary.Remove(moduleContext);
        }

        /// <summary>
        /// Checks whether a module context is already assigned to the item.
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        /// <returns>True a mapping exists, false otherwise.</returns>
        public bool IsAssociatedWithModule(IModuleContext moduleContext)
        {
            return Dictionary.ContainsKey(moduleContext);
        }

        /// <summary>
        /// Raises the AddPage event.
        /// </summary>
        /// <param name="pageContext">The page context.</param>
        private void OnAddPage(IPageContext pageContext)
        {
            AddPage?.Invoke(this, pageContext);
        }

        /// <summary>
        /// Raises the RemovePage event.
        /// </summary>
        /// <param name="pageContext">The page context.</param>
        private void OnRemovePage(IPageContext pageContext)
        {
            RemovePage?.Invoke(this, pageContext);
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (Delegate d in AddPage.GetInvocationList())
            {
                AddPage -= (EventHandler<IPageContext>)d;
            }

            foreach (Delegate d in RemovePage.GetInvocationList())
            {
                RemovePage -= (EventHandler<IPageContext>)d;
            }
        }

        /// <summary>
        /// Convert the resource element to a string.
        /// </summary>
        /// <returns>The resource element in its string representation.</returns>
        public override string ToString()
        {
            return $"Page '{PageId}'";
        }
    }
}
