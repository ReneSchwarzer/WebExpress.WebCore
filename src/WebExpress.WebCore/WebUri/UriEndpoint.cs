using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// An Uri represents a complete, fully qualified Uniform Resource Identifier (URI) that uniquely 
    /// identifies a endpoint (see RFC 3986).
    /// This interface encapsulates all components of a typical URI, such as the scheme (e.g., "http", "https"),
    /// the authority (e.g., "example.com"), path segments, query parameters, and fragment. It provides the external
    /// address used for resource identification and linking (e.g., "http://example.com/users/123").
    /// </summary>
    public partial class UriEndpoint : IUri
    {
        /// <summary>
        /// A regular expression to match URIs.
        /// </summary>
        /// <returns>A Regex object for matching URIs.</returns>
        [GeneratedRegex("^([a-zA-Z0-9+.-]+):(?://(?:((?:[a-zA-Z0-9-._~!$&'()*+,;=:]|%[0-9a-fA-F]{2})*)@)?((?:[a-zA-Z0-9-._~!$&'()*+,;=]|%[0-9a-fA-F]{2})*)(?::(\\d*))?(.*)?)$")]
        private static partial Regex UriRegex();

        /// <summary>
        /// Regular expression to match relative URIs.
        /// </summary>
        /// <returns>A Regex object for matching relative URIs.</returns>
        [GeneratedRegex(@"^(\/([a-zA-Z0-9-+*%()=._/$]*))?(\?([a-zA-Z0-9-+*%()=._/$&]*))?(#([a-zA-Z0-9-+*%()=._/$]*))?$")]
        private static partial Regex RelativeUriRegex();

        /// <summary>
        /// The scheme (e.g. Http, FTP).
        /// </summary>
        public UriScheme Scheme { get; set; } = UriScheme.Http;

        /// <summary>
        /// The authority (e.g. user@example.com:8080).
        /// </summary>
        public UriAuthority Authority { get; set; }

        /// <summary>
        /// The path (e.g. /over/there).
        /// </summary>
        public IEnumerable<IUriPathSegment> PathSegments { get; private set; } = [new UriPathSegmentRoot()];

        /// <summary>
        /// Returns or sets the base path of the endpoint's URI.
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
        public IEnumerable<UriQuery> Query { get; } = [];

        /// <summary>
        /// References a position within a resource (e.g. #Anchor).
        /// </summary>
        public string Fragment { get; set; }

        /// <summary>
        /// Returns the display string of the Uri
        /// </summary>
        public virtual string Display
        {
            get
            {
                if (PathSegments.LastOrDefault() is IUriPathSegment last)
                {
                    return last?.Display;
                }

                return null;
            }

            set
            {
                if (PathSegments.LastOrDefault() is IUriPathSegment last)
                {
                    last.Display = value;
                }
            }
        }

        /// <summary>
        /// Determines if the uri is empty.
        /// </summary>
        public bool Empty => !PathSegments.Any();

        /// <summary>
        /// Determines if the Uri is the root.
        /// </summary>
        public bool IsRoot => PathSegments.Count() == 1;

        /// <summary>
        /// Checks if it is a relative uri.
        /// </summary>
        public bool IsRelative => Authority == null;

        /// <summary>
        /// Retrieves a collection of variables represented as key-value pairs.
        /// </summary>
        public IDictionary<string, string> Parameters
        {
            get
            {
                var dic = new Dictionary<string, string>();

                foreach (var path in PathSegments)
                {
                    if (path is IUriPathSegmentVariable variable)
                    {
                        if (!dic.ContainsKey(variable.VariableName?.ToLower()))
                        {
                            dic.Add(variable.VariableName?.ToLower(), variable.Value);
                        }
                    }
                }

                return dic;
            }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UriEndpoint()
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="scheme">The scheme (e.g. Http, FTP).</param>
        /// <param name="authority">The authority (e.g. user@example.com:8080).</param>
        /// <param name="uri">The uri.</param>
        public UriEndpoint(UriScheme scheme, UriAuthority authority, string uri)
            : this(uri)
        {
            Scheme = scheme;
            Authority = authority;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="uri">The uri.</param>
        public UriEndpoint(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri) || uri == "/")
            {
                return;
            }

            if (Enum.GetNames<UriScheme>().Where(x => uri.StartsWith(x, StringComparison.OrdinalIgnoreCase)).Any())
            {
                var match = UriRegex().Match(uri);

                try
                {
                    Scheme = Enum.Parse<UriScheme>(match.Groups[1].Value, true);
                }
                catch
                {
                    Scheme = UriScheme.Http;
                }

                Authority = new UriAuthority()
                {
                    User = match.Groups[2].Success ? match.Groups[2].Value : null,
                    Host = match.Groups[3].Success ? match.Groups[3].Value : null,
                    Port = match.Groups[4].Success ? Convert.ToInt32(match.Groups[4].Value) : null
                };

                uri = match.Groups[5].Value;

            }

            var relativeMatch = RelativeUriRegex().Match(uri);

            foreach (var p in relativeMatch.Groups[2].Value.Split('/', StringSplitOptions.RemoveEmptyEntries))
            {
                PathSegments = PathSegments.Concat([new UriPathSegmentConstant(p)]);
            }

            foreach (var q in relativeMatch.Groups[4].Success ? relativeMatch.Groups[4].Value?.Split('&') : [])
            {
                var item = q.Split('=');

                Query = Query.Concat([new UriQuery(item[0], item.Length > 1 ? item[1] : null)]);
            }

            Fragment = relativeMatch.Groups[6].Success ? relativeMatch.Groups[6].Value : null;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="uri">The uri.</param>
        public UriEndpoint(IUri uri)
        {
            Scheme = uri?.Scheme ?? UriScheme.Http;
            Authority = uri?.Authority;
            PathSegments = uri?.PathSegments.Select(x => x.Copy()) ?? [];
            Query = uri?.Query.Select(x => new UriQuery(x.Key, x.Value)) ?? [];
            Fragment = uri?.Fragment;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="segments">The path segments.</param>
        public UriEndpoint(params IUriPathSegment[] segments)
        {
            PathSegments = PathSegments.Concat([new UriPathSegmentRoot()]);

            foreach (var segment in segments.Where(x => x is not UriPathSegmentRoot))
            {
                PathSegments = PathSegments.Concat([segment]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <param name="segments">The path segments.</param>
        public UriEndpoint(IUri uri, IEnumerable<IUriPathSegment> segments)
            : this(uri.Scheme, uri.Authority, uri.Fragment, uri.Query, segments)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <param name="segments">The path segments.</param>
        /// <param name="extendedSegments">Other segments.</param>
        public UriEndpoint(IUri uri, IEnumerable<IUriPathSegment> segments, IEnumerable<IUriPathSegment> extendedSegments)
            : this(uri.Scheme, uri.Authority, uri.Fragment, uri.Query, extendedSegments != null ? segments.Union(extendedSegments) : segments)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="scheme">The scheme (e.g. Http, FTP).</param>
        /// <param name="authority">The authority (e.g. user@example.com:8080).</param>
        /// <param name="fragment">References a position within a resource (e.g. #Anchor).</param>
        /// <param name="query">The query part (e.g. ?title=Uniform_Resource_Identifier).</param>
        /// <param name="segments">The path segments.</param>
        public UriEndpoint(UriScheme scheme, UriAuthority authority, string fragment, IEnumerable<UriQuery> query, IEnumerable<IUriPathSegment> segments)
        {
            Scheme = scheme;
            Authority = authority;
            PathSegments = PathSegments.Concat(segments?.Where(x => x is not UriPathSegmentRoot).Select(x => x.Copy()) ?? []);
            Query = query.Select(x => new UriQuery(x.Key, x.Value));
            Fragment = fragment;
        }

        /// <summary>
        /// Concatenates the given path segment to the current URI and returns a new instance of IUri with the updated path.
        /// </summary>
        /// <param name="segment">The path segment to be concatenated with the existing URI.</param>
        /// <returns>A new IUri instance representing the URI after concatenation.</returns>
        public virtual IUri Concat(string segment)
        {
            if (string.IsNullOrWhiteSpace(segment))
            {
                return this;
            }

            var copy = new UriEndpoint((IUri)this);
            copy.PathSegments = copy.PathSegments
                .Concat(segment.Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new UriPathSegmentConstant(x)));

            return copy;
        }

        /// <summary>
        /// Concatenates the given path segment to the current URI and returns a new instance of IUri with the updated path.
        /// </summary>
        /// <param name="segments">An array of path segments to be concatenated to the existing URI.</param>
        /// <returns>A new IUri instance representing the URI after concatenation.</returns>
        public virtual IUri Concat(params IUriPathSegment[] segments)
        {
            if (segments.Length == 0)
            {
                return this;
            }

            var copy = new UriEndpoint((IUri)this);
            copy.PathSegments = copy.PathSegments
                .Select(x => x.Copy());

            return copy;
        }

        /// <summary>
        /// Return a shortened uri containing n-elements.
        /// count greater than 0 count elements are included
        /// count less than 0 count elements are truncated
        /// count = 0 an empty uri is returned
        /// </summary>
        /// <param name="count">The count of elements to include or truncate.</param>
        /// <returns>The sub uri with the specified number of elements.</returns>
        public virtual IUri Take(int count)
        {
            var copy = new UriEndpoint((IUri)this);
            var path = copy.PathSegments.ToList();
            copy.PathSegments = [];

            if (count == 0)
            {
                return new UriEndpoint();
            }
            else if (count > 0)
            {
                copy.PathSegments = copy.PathSegments.Concat(path.Take(count));
            }
            else if (count < 0 && Math.Abs(count) < path.Count)
            {
                copy.PathSegments = copy.PathSegments.Concat(path.Take(path.Count + count));
            }
            else
            {
                return null;
            }

            return copy;
        }

        /// <summary>
        /// Return a shortened uri by not including the first n elements.
        /// count greater than 0 count elements are skipped
        /// count less than or equals 0 an empty Uri is returned
        /// </summary>
        /// <param name="count">The count of elements to skip.</param>
        /// <returns>The sub uri after skipping the specified number of elements.</returns>
        public IUri Skip(int count)
        {
            if (count >= PathSegments.Count())
            {
                return null;
            }
            if (count > 0)
            {
                var copy = new UriEndpoint((IUri)this);
                var path = copy.PathSegments.ToList();
                copy.PathSegments = [];
                copy.PathSegments = copy.PathSegments.Concat(path.Skip(count));

                return copy;
            }

            return new UriEndpoint((IUri)this);
        }

        /// <summary>
        /// Determines whether the given segment is part of the uri.
        /// </summary>
        /// <param name="segment">The segment to be tested.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public virtual bool Contains(string segment)
        {
            return PathSegments.Where(x => x.Value.Equals(segment, StringComparison.OrdinalIgnoreCase)).Any();
        }

        /// <summary>
        /// Checks whether a given uri is part of that uri.
        /// </summary>
        /// <param name="uri">The Uri to be checked.</param>
        /// <returns>true if part of the uri, false otherwise.</returns>
        public bool StartsWith(IUri uri)
        {
            return ToString().StartsWith(uri.ToString());
        }

        /// <summary>
        /// Creates a new endpoint uri and fills it with the given parameters.
        /// </summary>
        /// <param name="parameters">The parameters that fill in the variable parts of the uri.</param>
        /// <returns>A new endpoint uri with the populated parameters.</returns>
        public IUri SetParameters(params WebMessage.Parameter[] parameters)
        {
            var pathSegments = PathSegments.AsEnumerable();

            foreach (var parameter in parameters)
            {
                pathSegments = pathSegments.Select(x =>
                {
                    if (x is IUriPathSegmentVariable variable &&
                        variable.VariableName.Equals(parameter?.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        var copy = variable.Copy() as IUriPathSegmentVariable;
                        copy.Value = parameter.Value;

                        return copy;
                    }

                    return x;
                });
            }

            return new UriEndpoint(this, pathSegments);
        }

        /// <summary>
        /// Combines the specified uris into a compound uri.
        /// </summary>
        /// <param name="uris">The uris to be combine.</param>
        /// <returns>A combined uri.</returns>
        public static IUri Combine(params string[] uris)
        {
            var copy = new UriEndpoint();

            copy.PathSegments = copy.PathSegments
                .Concat(uris.Where(x => !string.IsNullOrWhiteSpace(x))
                .SelectMany(x => x.Split('/', StringSplitOptions.RemoveEmptyEntries))
                .Select(x => new UriPathSegmentConstant(x) as IUriPathSegment));

            return copy;
        }

        /// <summary>
        /// Combines the specified uris into a compound uri.
        /// </summary>
        /// <param name="uris">The uris to be combine.</param>
        /// <returns>A combined uri.</returns>
        public static IUri Combine(params IUri[] uris)
        {
            var copy = new UriEndpoint(uris.FirstOrDefault());
            copy.PathSegments = copy.PathSegments
                .Concat(uris.Skip(1).SelectMany(x => x.PathSegments.Skip(1)));

            return copy;
        }

        /// <summary>
        /// Combines the specified uris into a compound uri.
        /// </summary>
        /// <param name="uri">The first uri to be combine.</param>
        /// <param name="uris">The uris to be combine.</param>
        /// <returns>A combined uri.</returns>
        public static IUri Combine(IUri uri, params string[] uris)
        {
            var copy = new UriEndpoint(uri);
            copy.PathSegments = copy.PathSegments
                .Concat(uris.Where(x => !string.IsNullOrWhiteSpace(x))
                .SelectMany(x => x.Split('/', StringSplitOptions.RemoveEmptyEntries))
                .Select(x => new UriPathSegmentConstant(x) as IUriPathSegment));

            return copy;
        }

        /// <summary>
        /// Sets the fragment component of the URI.
        /// </summary>
        /// <param name="fragment">The fragment to set (e.g., "section1").</param>
        /// <returns>A new IUri instance with the updated fragment. The original URI remains unchanged.</returns>
        public IUri SetFragment(string fragment)
        {
            return new UriEndpoint(Scheme, Authority, fragment, Query, PathSegments);
        }

        /// <summary>
        /// Converts a resource uri to a normal uri.
        /// </summary>
        /// <param name="uri">The uri to convert.</param>
        public static implicit operator string(UriEndpoint uri)
        {
            return uri?.ToString();
        }

        /// <summary>
        /// Converts the uri to a string.
        /// </summary>
        /// <returns>A string that represents the current uri.</returns>
        public override string ToString()
        {
            var defaultPort = Scheme switch
            {
                UriScheme.Http => 80,
                UriScheme.Https => 443,
                UriScheme.FTP => 21,
                UriScheme.Ldap => 389,
                UriScheme.Ldaps => 636,
                _ => -1

            };

            var scheme = Scheme.ToString("g").ToLower() + ":";
            var authority = Authority?.ToString(defaultPort);
            var uri = "/" + string.Join
            (
                "/",
                PathSegments.Where(x => x is not UriPathSegmentRoot)
                    .Select(x => x.ToString().TrimStart('/'))
            ).TrimEnd('/');

            if (Query.Any())
            {
                uri += "?" + string.Join("&", Query.Select(x => x.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(Fragment))
            {
                uri += "#" + Fragment;
            }

            return Scheme switch
            {
                UriScheme.Mailto => string.Format("{0}{1}", scheme, authority),
                _ => IsRelative ? uri : string.Format("{0}{1}{2}", scheme, authority, uri),
            };
        }
    }
}