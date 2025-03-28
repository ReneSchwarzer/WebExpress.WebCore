using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSitemap
{
    /// <summary>
    /// The interface of the sitemap manager.
    /// </summary>
    public interface ISitemapManager : IComponentManager
    {
        /// <summary>
        /// Returns the side map.
        /// </summary>
        IEnumerable<IEndpointContext> SiteMap { get; }

        /// <summary>
        /// Rebuilds the sitemap.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Locates the resource associated with the Uri.
        /// </summary>
        /// <param name="requestUri">The Uri.</param>
        /// <param name="searchContext">The search context.</param>
        /// <returns>The search result with the found resource or null</returns>
        SearchResult SearchResource(Uri requestUri, SearchContext searchContext);

        /// <summary>
        /// Returns the URI for this type based on the sitemap configuration, taking into account the specific context in which the URI is valid.
        /// </summary>
        /// <typeparam name="TEndpoint">The class from which the URI is to be determined. URI route must not have any dynamic components (such as '/a/guid/b').</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="parameters">The parameters to be considered for the uri.</param>
        /// <returns>Returns the URI taking into account the context, or null if no valid URI is found.</returns>
        IUri GetUri<TEndpoint>(IApplicationContext applicationContext, params Parameter[] parameters)
            where TEndpoint : IEndpoint;

        /// <summary>
        /// Returns the URI for this type based on the sitemap configuration, taking into account the specific context in which the URI is valid.
        /// </summary>
        /// <param name="endpointType">The endpoint type.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="parameters">The parameters to be considered for the uri.</param>
        /// <returns>Returns the URI taking into account the context, or null if no valid URI is found.</returns>
        IUri GetUri(Type endpointType, IApplicationContext applicationContext, params Parameter[] parameters);

        /// <summary>
        /// Returns the URI for this type based on the sitemap configuration, taking into account the specific context in which the URI is valid.
        /// </summary>
        /// <typeparam name="TEndpoint">The class from which the URI is to be determined. URI route must not have any dynamic components (such as '/a/guid/b').</typeparam>
        /// <param name="endpointContext">The endpoint context.</param>
        /// <returns>Returns the URI taking into account the context, or null if no valid URI is found.</returns>
        IUri GetUri<TEndpoint>(IEndpointContext endpointContext)
            where TEndpoint : IEndpoint;

        /// <summary>
        /// Retrieves the endpoint context associated with the given URI.
        /// </summary>
        /// <param name="uri">The URI resource to search for.</param>
        /// <returns>The endpoint context if found, otherwise null.</returns>
        IEndpointContext GetEndpoint(UriEndpoint uri);
    }
}
