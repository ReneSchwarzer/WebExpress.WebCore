using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// A resource uri (e.g. /image.png).
    /// </summary>
    public class UriResource
    {
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
        public ICollection<IUriPathSegment> PathSegments { get; } = new List<IUriPathSegment>();

        /// <summary>
        /// Returns the extended path. The extended path is the postfix of the resource's path.
        /// </summary>
        public UriResource ExtendedPath
        {
            get
            {
                return new UriResource(Skip(ResourceRoot.PathSegments.Count()).PathSegments?.ToArray());
            }
        }

        /// <summary>
        /// The query part (e.g. ?title=Uniform_Resource_Identifier&action=submit).
        /// </summary>
        public ICollection<UriQuerry> Query { get; } = new List<UriQuerry>();

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
        /// Returns the root of the resource.
        /// </summary>
        public virtual UriResource ResourceRoot { get; set; }

        /// <summary>
        /// Returns the root of the module.
        /// </summary>
        public virtual UriResource ModuleRoot { get; set; }

        /// <summary>
        /// Returns the root of the application.
        /// </summary>
        public virtual UriResource ApplicationRoot { get; set; }

        /// <summary>
        /// Returns the root of the server.
        /// </summary>
        public virtual UriResource ServerRoot { get; set; }

        /// <summary>
        /// Determines if the Uri is the root.
        /// </summary>
        public bool IsRoot => PathSegments.Count() == 1;

        /// <summary>
        /// Checks if it is a relative uri.
        /// </summary>
        public bool IsRelative => Authority == null;

        /// <summary>
        /// Returns the variables.
        /// </summary>
        public Dictionary<string, string> Parameters
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
        /// Constructor
        /// </summary>
        public UriResource()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scheme">The scheme (e.g. Http, FTP).</param>
        /// <param name="authority">The authority (e.g. user@example.com:8080).</param>
        /// <param name="uri">The uri.</param>
        public UriResource(UriScheme scheme, UriAuthority authority, string uri)
            : this(uri)
        {
            Scheme = scheme;
            Authority = authority;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">The uri.</param>
        public UriResource(string uri)
        {
            if (uri == null) return;

            if (Enum.GetNames(typeof(UriScheme)).Where(x => uri.StartsWith(x, StringComparison.OrdinalIgnoreCase)).Any())
            {
                var match = Regex.Match(uri, "^([a-z0-9+.-]+):(?://(?:((?:[a-z0-9-._~!$&'()*+,;=:]|%[0-9A-F]{2})*)@)?((?:[a-z0-9-._~!$&'()*+,;=]|%[0-9A-F]{2})*)(?::(\\d*))?(.*)?)$");

                try
                {
                    Scheme = (UriScheme)Enum.Parse(typeof(UriScheme), match.Groups[1].Value, true);
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

            var relativeMatch = Regex.Match(uri, @"^(\/([a-zA-Z0-9-+*%()=._/$]*))(#([a-zA-Z0-9-+*%()=._/$]*))?(\?(.*))?$");

            PathSegments.Add(new UriPathSegmentRoot());

            foreach (var p in relativeMatch.Groups[2].Value.Split('/', StringSplitOptions.RemoveEmptyEntries))
            {
                PathSegments.Add(new UriPathSegmentConstant(p));
            }

            Fragment = relativeMatch.Groups[4].Success ? relativeMatch.Groups[4].Value : null;

            foreach (var q in relativeMatch.Groups[6].Success ? relativeMatch.Groups[6].Value?.Split('&') : Enumerable.Empty<string>())
            {
                var item = q.Split('=');

                Query.Add(new UriQuerry(item[0], item.Length > 1 ? item[1] : null));
            }
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="uri">The uri.</param>
        public UriResource(UriResource uri)
        {
            Scheme = uri.Scheme;
            Authority = uri.Authority;
            PathSegments = uri.PathSegments.Select(x => x.Copy()).ToList();
            Query = uri.Query.Select(x => new UriQuerry(x.Key, x.Value)).ToList();
            Fragment = uri.Fragment;
            ServerRoot = uri.ServerRoot;
            ApplicationRoot = uri.ApplicationRoot;
            ModuleRoot = uri.ModuleRoot;
            ResourceRoot = uri.ResourceRoot;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="segments">The path segments.</param>
        public UriResource(params IUriPathSegment[] segments)
        {
            PathSegments.Add(new UriPathSegmentRoot());

            foreach (var segment in segments.Where(x => !(x is UriPathSegmentRoot)))
            {
                PathSegments.Add(segment);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <param name="segments">The path segments.</param>
        public UriResource(UriResource uri, IEnumerable<IUriPathSegment> segments)
            : this(uri.Scheme, uri.Authority, uri.Fragment, uri.Query, segments)
        {
            ServerRoot = uri.ServerRoot;
            ApplicationRoot = uri.ApplicationRoot;
            ModuleRoot = uri.ModuleRoot;
            ResourceRoot = uri.ResourceRoot;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <param name="segments">The path segments.</param>
        /// <param name="extendedSegments">Other segments.</param>
        public UriResource(UriResource uri, IEnumerable<IUriPathSegment> segments, IEnumerable<IUriPathSegment> extendedSegments)
            : this(uri.Scheme, uri.Authority, uri.Fragment, uri.Query, extendedSegments != null ? segments.Union(extendedSegments) : segments)
        {
            ServerRoot = uri.ServerRoot;
            ApplicationRoot = uri.ApplicationRoot;
            ModuleRoot = uri.ModuleRoot;
            ResourceRoot = uri.ResourceRoot;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scheme">The scheme (e.g. Http, FTP).</param>
        /// <param name="authority">The authority (e.g. user@example.com:8080).</param>
        /// <param name="fragment">References a position within a resource (e.g. #Anchor).</param>
        /// <param name="query">The query part (e.g. ?title=Uniform_Resource_Identifier&action=submit).</param>
        /// <param name="segments">The path segments.</param>
        public UriResource(UriScheme scheme, UriAuthority authority, string fragment, IEnumerable<UriQuerry> query, IEnumerable<IUriPathSegment> segments)
        {
            Scheme = scheme;
            Authority = authority;
            PathSegments.Add(new UriPathSegmentRoot());

            foreach (var segment in segments != null ? segments.Where(x => !(x is UriPathSegmentRoot)) : Enumerable.Empty<IUriPathSegment>())
            {
                PathSegments.Add(segment.Copy());
            }

            Query = query.Select(x => new UriQuerry(x.Key, x.Value)).ToList();
            Fragment = fragment;
        }

        /// <summary>
        /// Adds a path element.
        /// </summary>
        /// <param name="path">The path to append.</param>
        /// <returns>The extended path.</returns>
        public virtual UriResource Append(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return this;
            }

            var copy = new UriResource(this);

            foreach (var p in path.Split('/', StringSplitOptions.RemoveEmptyEntries))
            {
                copy.PathSegments.Add(new UriPathSegmentConstant(p));
            }

            return copy;
        }

        /// <summary>
        /// Adds a path element.
        /// </summary>
        /// <param name="path">The path to append.</param>
        /// <returns>The extended path.</returns>
        public virtual UriResource Append(IUriPathSegment path)
        {
            if (path == null || path.IsEmpty)
            {
                return this;
            }

            var copy = new UriResource(this);

            copy.PathSegments.Add(path);

            return copy;
        }

        /// <summary>
        /// Return a shortened uri containing n-elements.
        /// count > 0 count elements are included
        /// count < 0 count elements are truncated
        /// count = 0 an empty uri is returned
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>The sub uri.</returns>
        public virtual UriResource Take(int count)
        {
            var copy = new UriResource(this);
            var path = copy.PathSegments.ToList();
            copy.PathSegments.Clear();

            if (count == 0)
            {
                return new UriResource();
            }
            else if (count > 0)
            {
                (copy.PathSegments as List<IUriPathSegment>).AddRange(path.Take(count));
            }
            else if (count < 0 && Math.Abs(count) < path.Count)
            {
                (copy.PathSegments as List<IUriPathSegment>).AddRange(path.Take(path.Count + count));
            }
            else
            {
                return null;
            }

            return copy;
        }

        /// <summary>
        /// Return a shortened uri by not including the first n elements.
        /// count > 0 count elements are skipped
        /// count <= 0 an empty Uri is returned
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>The sub uri.</returns>
        public UriResource Skip(int count)
        {
            if (count >= PathSegments.Count)
            {
                return null;
            }
            if (count > 0)
            {
                var copy = new UriResource(this);
                var path = copy.PathSegments.ToList();
                copy.PathSegments.Clear();
                (copy.PathSegments as List<IUriPathSegment>).AddRange(path.Skip(count));

                return copy;
            }

            return new UriResource(this);
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
        public bool StartsWith(UriResource uri)
        {
            return ToString().StartsWith(uri.ToString());
        }

        /// <summary>
        /// Creates a new resource uri and fills it with the given parameters.
        /// </summary>
        /// <param name="parameters">The parameters that fill in the variable parts of the uri.</param>
        /// <returns>A new resource uri with the populated parameters.</returns>
        public UriResource SetParameters(params WebMessage.Parameter[] parameters)
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

            return new UriResource(this, pathSegments);
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
                PathSegments.Where(x => !(x is UriPathSegmentRoot)).Select(x => x.ToString())
            );

            if (!string.IsNullOrWhiteSpace(Fragment))
            {
                uri += "#" + Fragment;
            }

            if (Query.Any())
            {
                uri += "?" + string.Join("&", Query.Select(x => $"{x.Key}={x.Value}"));
            }

            return Scheme switch
            {
                UriScheme.Mailto => string.Format("{0}{1}", scheme, authority),
                _ => IsRelative ? uri : string.Format("{0}{1}{2}", scheme, authority, uri),
            };
        }

        /// <summary>
        /// Combines the specified uris into a compound uri.
        /// </summary>
        /// <param name="uris">The uris to be combine.</param>
        /// <returns>A combined uri.</returns>
        public static UriResource Combine(params string[] uris)
        {
            var uri = new UriResource();

            (uri.PathSegments as List<IUriPathSegment>).AddRange(uris.Where(x => !string.IsNullOrWhiteSpace(x))
                            .SelectMany(x => x.Split('/', StringSplitOptions.RemoveEmptyEntries))
                            .Select(x => new UriPathSegmentConstant(x) as IUriPathSegment));

            return uri;
        }

        /// <summary>
        /// Combines the specified uris into a compound uri.
        /// </summary>
        /// <param name="uris">The uris to be combine.</param>
        /// <returns>A combined uri.</returns>
        public static UriResource Combine(params UriResource[] uris)
        {
            var uri = new UriResource(uris.FirstOrDefault());
            (uri.PathSegments as List<IUriPathSegment>).AddRange(uris.Skip(1).SelectMany(x => x.PathSegments.Skip(1)));

            return uri;
        }

        /// <summary>
        /// Combines the specified uris into a compound uri.
        /// </summary>
        /// <param name="uri">The first uri to be combine.</param>
        /// <param name="uris">The uris to be combine.</param>
        /// <returns>A combined uri.</returns>
        public static UriResource Combine(UriResource uri, params string[] uris)
        {
            var copy = new UriResource(uri);
            (copy.PathSegments as List<IUriPathSegment>).AddRange(uris.Where(x => !string.IsNullOrWhiteSpace(x))
                    .SelectMany(x => x.Split('/', StringSplitOptions.RemoveEmptyEntries))
                    .Select(x => new UriPathSegmentConstant(x) as IUriPathSegment));
            return copy;
        }

        /// <summary>
        /// Converts a resource uri to a normal uri.
        /// </summary>
        /// <param name="uri">The uri to convert.</param>
        public static implicit operator string(UriResource uri)
        {
            return uri?.ToString();
        }
    }
}