using System.Collections.Generic;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebSitemap.Model;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSitemap
{
    /// <summary>
    /// Represents the search result within the site map.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Returns the context of the endpoint.
        /// </summary>
        public IEndpointContext EndpointContext { get; internal set; }

        /// <summary>
        /// Returns the search context.
        /// </summary>
        public SearchContext SearchContext { get; internal set; }

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
    }
}
