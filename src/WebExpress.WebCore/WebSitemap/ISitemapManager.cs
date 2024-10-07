using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebModule;
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
        /// Registers an endpoint manager.
        /// </summary>
        /// <typeparam name="T">The type of the endpoint context.</typeparam>
        /// <param name="registration">The registration details containing the callback functions.</param>
        void Register<T>(EndpointRegistration registration) where T : IEndpointContext;

        /// <summary>
        /// Removes the registration for a specific endpoint manager.
        /// </summary>
        /// <typeparam name="T">The type of the endpoint context.</typeparam>
        void Remove<T>() where T : IEndpointContext;

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
        /// Determines the Uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <typeparam name="T">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid>/b').</typeparam>
        /// <paramref name="parameters"/>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        UriResource GetUri<T>(params Parameter[] parameters) where T : IEndpoint;

        /// <summary>
        /// Determines the Uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="parameters">The parameters to be considered for the URI.</param>
        /// <returns>Returns the URI taking into account the context, or null if no valid URI is found.</returns>
        UriResource GetUri(Type resourceType, params Parameter[] parameters);

        /// <summary>
        /// Determines the Uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <typeparam name="T">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid>/b').</typeparam>
        /// <param name="resourceContext">The module context.</param>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        UriResource GetUri<T>(IModuleContext moduleContext) where T : IEndpoint;

        /// <summary>
        /// Determines the Uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <typeparam name="T">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid>/b').</typeparam>
        /// <param name="endpointContext">The module context.</param>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        UriResource GetUri<T>(IEndpointContext endpointContext) where T : IEndpoint;
    }
}
