using System.Collections.Generic;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// An Uri represents a complete, fully qualified Uniform Resource Identifier (URI) that uniquely identifies a endpoint.
    /// 
    /// This interface encapsulates all components of a typical URI, such as the scheme (e.g., "http", "https"),
    /// the authority (e.g., "example.com"), path segments, query parameters, and fragment. It provides the external
    /// address used for resource identification and linking (e.g., "http://example.com/users/123").
    /// </summary>
    public interface IUri
    {
        /// <summary>
        /// The scheme (e.g. Http, FTP).
        /// </summary>
        UriScheme Scheme { get; }

        /// <summary>
        /// The authority (e.g. user@example.com:8080).
        /// </summary>
        UriAuthority Authority { get; }

        /// <summary>
        /// The path (e.g. /over/there).
        /// </summary>
        IEnumerable<IUriPathSegment> PathSegments { get; }

        /// <summary>
        /// Returns the extended segment of the endpoint's path, which is included only when the endpoint class has the IncludeSubPaths attribute enabled.
        /// For example, if the core endpoint is "http://example.com/server/app/endpoint" and the extended segment is "extended",
        /// the complete URI becomes "http://example.com/server/app/endpoint/extended". In this case, the property returns the "extended" part.
        /// </summary>
        IUri ExtendedPath { get; }

        /// <summary>
        /// The query part (e.g. ?title=Uniform_Resource_Identifier).
        /// </summary>
        IEnumerable<UriQuery> Query { get; }

        /// <summary>
        /// References a position within a resource (e.g. #Anchor).
        /// </summary>
        string Fragment { get; }

        /// <summary>
        /// Returns the display string of the Uri
        /// </summary>
        string Display { get; }

        /// <summary>
        /// Determines if the uri is empty.
        /// </summary>
        bool Empty { get; }

        /// <summary>
        /// Retrieves the base URI of the endpoint. When the IncludeSubPaths attribute is enabled on the endpoint class,
        /// the complete URI may include extra path segments. For example, the core endpoint could be
        /// "http://example.com/server/app/endpoint", but with IncludeSubPaths enabled, the full URI might become
        /// "http://example.com/server/app/endpoint/extended". In such cases, this property returns only the base URI:
        /// "http://example.com/server/app/endpoint".
        /// </summary>
        IUri EndpointRoot { get; }

        /// <summary>
        /// Returns the root of the application.
        /// </summary>
        IUri ApplicationRoot { get; }

        /// <summary>
        /// Returns the root of the server.
        /// </summary>
        IUri ServerRoot { get; }

        /// <summary>
        /// Determines if the Uri is the root.
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// Checks if it is a relative uri.
        /// </summary>
        bool IsRelative { get; }

        /// <summary>
        /// Retrieves a collection of variables represented as key-value pairs.
        /// </summary>
        IDictionary<string, string> Parameters { get; }

        /// <summary>
        /// Concatenates the given path segment to the current URI and returns a new instance of IUri with the updated path.
        /// </summary>
        /// <param name="segment">The path segment to be concatenated with the existing URI.</param>
        /// <returns>A new IUri instance representing the URI after concatenation.</returns>
        IUri Concat(string segment);

        /// <summary>
        /// Concatenates the given path segment to the current URI and returns a new instance of IUri with the updated path.
        /// </summary>
        /// <param name="segments">An array of path segments to be concatenated to the existing URI.</param>
        /// <returns>A new IUri instance representing the URI after concatenation.</returns>
        IUri Concat(params IUriPathSegment[] segments);

        /// <summary>
        /// Return a shortened uri containing n-elements.
        /// count greater than 0 count elements are included
        /// count less than 0 count elements are truncated
        /// count = 0 an empty uri is returned
        /// </summary>
        /// <param name="count">The count of elements to include or truncate.</param>
        /// <returns>The sub uri with the specified number of elements.</returns>
        IUri Take(int count);

        /// <summary>
        /// Return a shortened uri by not including the first n elements.
        /// count greater than 0 count elements are skipped
        /// count less than or equals 0 an empty Uri is returned
        /// </summary>
        /// <param name="count">The count of elements to skip.</param>
        /// <returns>The sub uri after skipping the specified number of elements.</returns>
        IUri Skip(int count);

        /// <summary>
        /// Determines whether the given segment is part of the uri.
        /// </summary>
        /// <param name="segment">The segment to be tested.</param>
        /// <returns>true if successful, false otherwise.</returns>
        bool Contains(string segment);

        /// <summary>
        /// Checks whether a given uri is part of that uri.
        /// </summary>
        /// <param name="uri">The Uri to be checked.</param>
        /// <returns>true if part of the uri, false otherwise.</returns>
        bool StartsWith(IUri uri);
    }
}
