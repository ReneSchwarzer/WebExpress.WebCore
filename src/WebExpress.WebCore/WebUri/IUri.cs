using System.Collections.Generic;
using WebExpress.WebCore.WebIcon;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebParameter;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// An Uri represents a complete, fully qualified Uniform Resource 
    /// Identifier (URI) that uniquely identifies a endpoint.
    /// This interface encapsulates all components of a typical URI, such as 
    /// the scheme (e.g., "http", "https"), the authority (e.g., "example.com"), 
    /// path segments, query parameters, and fragment. It provides the external
    /// address used for resource identification and linking 
    /// (e.g., "http://example.com/users/123").
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
        /// Gets or sets the base path of the endpoint's URI.
        /// The base path is included only when the endpoint class has the IncludeSubPaths attribute enabled.
        /// For example, if the complete URI is "http://example.com/server/app/endpoint/extended",
        /// the <c>BasePath</c> property will represent the "http://example.com/server/app/endpoint" portion of the URI.
        /// </summary>
        /// <value>
        /// The base path as an <see cref="IUri"/> object, or <c>null</c> if the IncludeSubPaths attribute is not enabled.
        /// </value>
        public IUri BasePath { get; set; }

        /// <summary>
        /// The query part (e.g. ?title=Uniform_Resource_Identifier).
        /// </summary>
        IEnumerable<IUriQuery> Query { get; }

        /// <summary>
        /// References a position within a resource (e.g. #Anchor).
        /// </summary>
        string Fragment { get; }

        /// <summary>
        /// Determines if the uri is empty.
        /// </summary>
        bool Empty { get; }

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
        /// Appends one or more query parameters to the current URI and returns a new instance with 
        /// the updated query
        /// string.
        /// </summary>
        /// <param name="query">An array of objects representing the query parameters to add. Each 
        /// parameter must not be null.</param>
        /// <returns>
        /// An uri instance containing the original URI with the specified query parameters appended.
        /// </returns>
        IUri Add(params IUriQuery[] query);

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
        /// Appends one or more query segments and returns a new URI instance with the 
        /// combined query parameters.
        /// </summary>
        /// <param name="query">
        /// An array representing the query segments to append. The order of segments
        /// determines their position in the resulting query string.
        /// </param>
        /// <returns>
        /// A new uri instance containing the original URI with the specified query segments appended.
        /// </returns>
        IUri Concat(params IUriQuery[] query);

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

        /// <summary>
        /// Sets the fragment component of the URI.
        /// </summary>
        /// <returns>A new IUri instance with the updated fragment. The original URI remains unchanged.</returns>
        IUri SetFragment(string fragment);

        /// <summary>
        /// Returns a string that represents the display text for the current instance.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        /// <returns>
        /// A string containing the display text associated with the instance. The 
        /// value may be empty if no display text is available.
        /// </returns>
        string GetDisplayText(IRenderContext renderContext);

        /// <summary>
        /// Returns an icon that visually represents the parameter within 
        /// the given render context.
        /// </summary>
        /// <param name="renderContext">
        /// The rendering context that provides information required to 
        /// determine the appropriate icon.
        /// </param>
        /// <returns>
        /// An icon associated with the current instance. The value may be 
        /// null or empty if no icon is available.
        /// </returns>
        IIcon GetIcon(IRenderContext renderContext);

        /// <summary>
        /// Creates a new endpoint uri and fills it with the given parameters.
        /// </summary>
        /// <param name="parameters">
        /// The parameters that fill in the variable parts of the uri.
        /// </param>
        /// <returns>
        /// A new endpoint uri with the populated parameters.
        /// </returns>
        IUri BindParameters(params IParameter[] parameters);

        /// <summary>
        /// Creates a new endpoint uri and fills it with the given parameters.
        /// </summary>
        /// <param name="parameters">
        /// The parameters that fill in the variable parts of the uri.
        /// </param>
        /// <returns>
        /// A new endpoint uri with the populated parameters.
        /// </returns>
        IUri BindParameters(IEnumerable<IParameter> parameters);

        /// <summary>
        /// Binds the parameters from the specified request to a URI instance.
        /// </summary>
        /// <param name="request">
        /// The request object containing the parameters to be bound to the URI. Cannot be null.
        /// </param>
        /// <returns>
        /// An new IUri instance that represents the URI with parameters bound from the request.
        /// </returns>
        IUri BindParameters(IRequest request);
    }
}
