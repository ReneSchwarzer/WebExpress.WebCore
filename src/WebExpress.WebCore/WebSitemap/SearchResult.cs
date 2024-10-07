using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSitemap
{
    /// <summary>
    /// Represents the search result within the site map.
    /// </summary>
    public class SearchResult
    {
        private readonly Func<IEndpoint, IEndpointContext, Request, Response> _handleRequest;

        /// <summary>
        /// Returns the endpoint id.
        /// </summary>
        public string EndpointId { get; internal set; }

        /// <summary>
        /// Returns the context of the endpoint.
        /// </summary>
        public IEndpointContext EndpointContext { get; internal set; }

        /// <summary>
        /// Returns the instance.
        /// </summary>
        public IEndpoint Instance { get; internal set; }

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

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="handleRequest">The function to handle requests.</param>
        internal SearchResult(Func<IEndpoint, IEndpointContext, Request, Response> handleRequest)
        {
            _handleRequest = handleRequest;
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        public Response Process(Request request)
        {
            if (_handleRequest != null)
            {
                return _handleRequest(Instance, EndpointContext, request);
            }

            return new ResponseBadRequest();
        }
    }
}
