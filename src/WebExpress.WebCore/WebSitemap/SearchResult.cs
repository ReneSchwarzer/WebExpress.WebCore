using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebResource;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSitemap
{
    /// <summary>
    /// Represents the search result within the site map.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Returns the resource id.
        /// </summary>
        public string Id { get; internal set; }

        /// <summary>
        /// Returns the resource title.
        /// </summary>
        public string Title { get; internal set; }

        /// <summary>
        /// Returns the instance.
        /// </summary>
        public IResource Instance { get; internal set; }

        /// <summary>
        /// Returns the search context.
        /// </summary>
        public SearchContext SearchContext { get; internal set; }

        /// <summary>
        /// Returns the context of the application.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns the context of the module.
        /// </summary>
        public IModuleContext ModuleContext { get; internal set; }

        /// <summary>
        /// Returns the context of the resource.
        /// </summary>
        public IResourceContext ResourceContext { get; internal set; }

        /// <summary>
        /// Returns the context where the resource exists.
        /// </summary>
        public IReadOnlyList<string> ResourceContextFilter { get; internal set; }

        /// <summary>
        /// Returns the path.
        /// </summary>
        /// <returns>The path.</returns>
        public ICollection<SitemapNode> Path { get; internal set; }

        /// <summary>
        /// Returns the uri.
        /// </summary>
        /// <returns>The uri.</returns>
        public UriResource Uri { get; internal set; }

        /// <summary>
        /// Constructor
        /// </summary>
        internal SearchResult()
        {

        }
    }
}
