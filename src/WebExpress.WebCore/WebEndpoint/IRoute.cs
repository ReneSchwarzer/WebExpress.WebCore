using System.Collections.Generic;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebEndpoint
{
    /// <summary>
    /// A route in WebExpress represents a logical internal path or pattern used to map endpoints within the sitemap.
    /// 
    /// While a URI (Uniform Resource Identifier) defines the complete, standardized external identifier of a resource
    /// (e.g., http://example.com/path?query=value#fragment) and thus represents the external address of an endpoint,
    /// a route represents the internal address of an endpoint that is used exclusively for internal purposes such as routing 
    /// and request processing.
    /// 
    /// A route contains neither a scheme, nor an authority, nor other external components. Furthermore, it may include 
    /// placeholders for dynamic segments (e.g., /users/{id}).
    /// </summary>
    public interface IRoute
    {
        /// <summary>
        /// Gets the segments of the route (e.g., /over/there).
        /// </summary>
        IEnumerable<IUriPathSegment> PathSegments { get; }

        /// <summary>
        /// Gets a string representation of the route.
        /// </summary>
        string Display { get; }

        /// <summary>
        /// Determines if the route is the root.
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// Concatenates the given path segment to the current route and returns a new instance of IRoute with the updated path.
        /// </summary>
        /// <param name="segment">The path segment to be concatenated with the existing route.</param>
        /// <returns>A new IRoute instance representing the route after concatenation.</returns>
        IRoute Concat(string segment);

        /// <summary>
        /// Concatenates the given path segment to the current route and returns a new instance of IRoute with the updated path.
        /// </summary>
        /// <param name="segments">An array of path segments to be concatenated to the existing route.</param>
        /// <returns>A new IRoute instance representing the route after concatenation.</returns>
        IRoute Concat(params IUriPathSegment[] segments);

        /// <summary>
        /// Removes a specified segment from the route and returns a new instance of IRoute with the updated path.
        /// </summary>
        /// <param name="segments">The path segment to be removed from the existing route.</param>
        /// <returns>A new IRoute instance representing the route after the segment removal.</returns>
        IRoute RemoveSegment(string segments);

        /// <summary>
        /// Converts the route to a URI.
        /// </summary>
        /// <param name="parameters">The parameters to be included in the URI.</param>
        /// <returns>An instance of IUri representing the route as a URI.</returns>
        IUri ToUri(params Parameter[] parameters);
    }
}
