using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebSitemap.Model;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSitemap
{
    /// <summary>
    /// The sitemap manager manages WebExpress elements, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public sealed class SitemapManager : ISitemapManager, ISystemComponent
    {
        private SitemapNode _root = new();
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;

        /// <summary>
        /// Returns the side map.
        /// </summary>
        public IEnumerable<IEndpointContext> SiteMap => _root.GetPreOrder()
            .Where(x => x != null)
            .Select(x => x.EndpointContext);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used via Reflection.")]
        private SitemapManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;
            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress.webcore:sitemapmanager.initialization")
            );
        }

        /// <summary>
        /// Rebuilds the sitemap.
        /// </summary>
        public void Refresh()
        {
            var newSiteMapNode = new SitemapNode() { PathSegment = new UriPathSegmentRoot() };

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress.webcore:sitemapmanager.refresh")
            );

            // applications
            var applications = _componentHub.ApplicationManager.Applications
                .Select(x => new
                {
                    ApplicationContext = x,
                    x.ContextPath.PathSegments
                })
                .OrderBy(x => x.PathSegments.Count);

            foreach (var application in applications)
            {
                MergeSitemap(newSiteMapNode, CreateSiteMap
                (
                    new Queue<IUriPathSegment>(application.PathSegments),
                    application.ApplicationContext
                ));
            }

            // endpoints
            var resources = _componentHub.EndpointManager.Endpoints
                .Select(x => new
                {
                    EndpointContext = x,
                    x.Uri.PathSegments
                })
                .OrderBy(x => x.PathSegments.Count);

            foreach (var item in resources)
            {
                MergeSitemap(newSiteMapNode, CreateSiteMap
                (
                    new Queue<IUriPathSegment>(item.PathSegments),
                    item.EndpointContext
                ));
            }

            _root = newSiteMapNode;

            Log();
        }

        /// <summary>
        /// Locates the resource associated with the Uri.
        /// </summary>
        /// <param name="requestUri">The Uri.</param>
        /// <param name="searchContext">The search context.</param>
        /// <returns>The search result with the found resource or null</returns>
        public SearchResult SearchResource(Uri requestUri, SearchContext searchContext)
        {
            var variables = new Dictionary<string, string>();
            var result = SearchNode
            (
                _root,
                new Queue<string>(requestUri.Segments.Select(x => x == "/" ? x : (x.EndsWith('/') ? x[..^1] : x))),
                new Queue<IUriPathSegment>(),
                searchContext
            );

            if (result != null && result.EndpointContext != null)
            {
                if (!result.EndpointContext.Conditions.Any() || result.EndpointContext.Conditions.All(x => x.Fulfillment(searchContext.HttpContext?.Request)))
                {
                    return result;
                }
            }

            // 404
            return result;
        }

        /// <summary>
        /// Determines the uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <typeparam name="T">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid/>/b').</typeparam>
        /// <paramref name="parameters"/>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        public UriResource GetUri<T>(params Parameter[] parameters) where T : IEndpoint
        {
            var endpointContexts = _componentHub.EndpointManager.GetEndpoints(typeof(T));

            var node = _root.GetPreOrder()
                .Where(x => endpointContexts.Contains(x.EndpointContext))
                .FirstOrDefault();

            return node?.EndpointContext?.Uri.SetParameters(parameters);
        }

        /// <summary>
        /// Determines the uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="parameters">The parameters to be considered for the URI.</param>
        /// <returns>Returns the URI taking into account the context, or null if no valid URI is found.</returns>
        public UriResource GetUri(Type resourceType, params Parameter[] parameters)
        {
            var endpointContexts = _componentHub.EndpointManager.GetEndpoints(resourceType);

            var node = _root.GetPreOrder()
                .Where(x => endpointContexts.Contains(x.EndpointContext))
                .FirstOrDefault();

            return node?.EndpointContext?.Uri.SetParameters(parameters);
        }

        /// <summary>
        /// Determines the uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <typeparam name="T">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid/>/b').</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        public UriResource GetUri<T>(IApplicationContext applicationContext) where T : IEndpoint
        {
            var endpointContexts = _componentHub.EndpointManager.GetEndpoints(typeof(T), applicationContext);

            var node = _root.GetPreOrder()
                .Where(x => endpointContexts.Contains(x.EndpointContext))
                .FirstOrDefault();

            return node?.EndpointContext?.Uri;
        }

        /// <summary>
        /// Determines the Uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <typeparam name="TEnpoint">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid/>/b').</typeparam>
        /// <param name="endpointContext">The endpoint context.</param>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        public UriResource GetUri<TEnpoint>(IEndpointContext endpointContext) where TEnpoint : IEndpoint
        {
            var endpointContexts = _componentHub.EndpointManager.GetEndpoints(typeof(TEnpoint), endpointContext.ApplicationContext)
                .Where(x => x.EndpointId.Equals(endpointContext.EndpointId));

            var node = _root.GetPreOrder()
                .Where(x => endpointContexts.Contains(x.EndpointContext))
                .FirstOrDefault();

            return node?.EndpointContext?.Uri;
        }

        /// <summary>
        /// Creates the sitemap. Works recursively.
        /// It is important for the algorithm that the addition of application is sorted 
        /// by the number of path segments in ascending order.
        /// </summary>
        /// <param name="contextPathSegments">The path segments of the context path.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>The sitemap root node.</returns>
        private static SitemapNode CreateSiteMap
        (
            Queue<IUriPathSegment> contextPathSegments,
            IApplicationContext applicationContext
        )
        {
            if (contextPathSegments.Peek() is UriPathSegmentRoot)
            {
                contextPathSegments.Dequeue();
            }

            var root = new SitemapNode() { PathSegment = new UriPathSegmentRoot() };
            var next = CreateSiteMap(contextPathSegments, applicationContext, root);

            if (next != null)
            {
                root.Children.Add(next);
            }

            return root;
        }

        /// <summary>
        /// Creates the sitemap. Works recursively.
        /// It is important for the algorithm that the addition of application is sorted 
        /// by the number of path segments in ascending order.
        /// </summary>
        /// <param name="contextPathSegments">The path segments of the context path.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="parent">The parent node or null if root.</param>
        /// <returns>The sitemap root node.</returns>
        private static SitemapNode CreateSiteMap
        (
            Queue<IUriPathSegment> contextPathSegments,
            IApplicationContext applicationContext,
            SitemapNode parent
        )
        {
            var pathSegment = contextPathSegments.Count != 0 ? contextPathSegments.Dequeue() : null;

            if (pathSegment == null)
            {
                return null;
            }

            var node = new SitemapNode()
            {
                PathSegment = pathSegment,
                Parent = parent,
            };

            if (contextPathSegments.Count != 0)
            {
                node.Children.Add(CreateSiteMap(contextPathSegments, applicationContext, node));
            }

            return node;
        }

        /// <summary>
        /// Creates the sitemap. Works recursively.
        /// It is important for the algorithm that the addition is sorted 
        /// by the number of path segments in ascending order.
        /// </summary>
        /// <param name="contextPathSegments">The path segments of the context path.</param>
        /// <param name="endpointContext">The endpoint context.</param>
        /// <returns>The sitemap root node.</returns>
        private static SitemapNode CreateSiteMap
        (
            Queue<IUriPathSegment> contextPathSegments,
            IEndpointContext endpointContext
        )
        {
            if (contextPathSegments.Peek() is UriPathSegmentRoot)
            {
                contextPathSegments.Dequeue();
            }

            var root = new SitemapNode() { PathSegment = new UriPathSegmentRoot() };
            var next = CreateSiteMap(contextPathSegments, endpointContext, root);

            if (next != null)
            {
                root.Children.Add(next);
            }
            else
            {
                root.EndpointContext = endpointContext;
            }

            return root;
        }

        /// <summary>
        /// Creates the sitemap. Works recursively.
        /// It is important for the algorithm that the addition of endpoint is sorted 
        /// by the number of path segments in ascending order.
        /// </summary>
        /// <param name="contextPathSegments">The path segments of the context path.</param>
        /// <param name="endpointContext">The endpoint context.</param>
        /// <param name="parent">The parent node or null if root.</param>
        /// <returns>The sitemap parent node.</returns>
        private static SitemapNode CreateSiteMap
        (
            Queue<IUriPathSegment> contextPathSegments,
            IEndpointContext endpointContext,
            SitemapNode parent = null
        )
        {
            var pathSegment = contextPathSegments.Count != 0 ? contextPathSegments.Dequeue() : null;

            if (pathSegment == null)
            {
                return null;
            }

            var node = new SitemapNode()
            {
                PathSegment = pathSegment,
                Parent = parent,
                EndpointContext = endpointContext
            };

            if (contextPathSegments.Count != 0)
            {
                node.Children.Add(CreateSiteMap(contextPathSegments, endpointContext, node));
            }

            return node;
        }

        /// <summary>
        /// Merges one sitemap with another. Works recursively.
        /// </summary>
        /// <param name="first">The first sitemap to be merged.</param>
        /// <param name="second">The second sitemap to be merged.</param>
        private static void MergeSitemap(SitemapNode first, SitemapNode second)
        {
            if (first.PathSegment.Equals(second.PathSegment))
            {
                foreach (var sc in second.Children)
                {
                    foreach (var fc in first.Children.Where(x => x.PathSegment.Equals(sc.PathSegment)))
                    {
                        fc.EndpointContext ??= sc.EndpointContext;

                        MergeSitemap(fc, sc);
                        return;
                    }

                    first.Children.Add(sc);
                }
            }

            return;
        }

        /// <summary>
        /// Locates the resource associated with the Uri. Works recursively.
        /// </summary>
        /// <param name="node">The sitemap node.</param>
        /// <param name="inPathSegments">The path segments.</param>
        /// <param name="outPathSegments">The path segments.</param>
        /// <param name="searchContext">The search context.</param>
        /// <returns>The search result with the found resource</returns>
        private static SearchResult SearchNode
        (
            SitemapNode node,
            Queue<string> inPathSegments,
            Queue<IUriPathSegment> outPathSegments,
            SearchContext searchContext
        )
        {
            var pathSegment = inPathSegments.Count != 0 ? inPathSegments.Dequeue() : null;
            var nextPathSegment = inPathSegments.Count != 0 ? inPathSegments.Peek() : null;

            if (IsMatched(node, pathSegment))
            {
                var copy = node.PathSegment.Copy();
                if (copy is UriPathSegmentVariable variable)
                {
                    variable.Value = pathSegment;
                }

                var type = node.EndpointContext?.GetType();

                outPathSegments.Enqueue(copy);

                if (nextPathSegment == null)
                {
                    return new SearchResult()
                    {
                        EndpointContext = node.EndpointContext,
                        SearchContext = searchContext,
                        Uri = new UriResource([.. outPathSegments])
                    };
                }
                else if (node.IsLeaf && nextPathSegment != null && node.EndpointContext != null && node.EndpointContext.IncludeSubPaths)
                {
                    return new SearchResult()
                    {
                        EndpointContext = node.EndpointContext,
                        SearchContext = searchContext,
                        Uri = new UriResource([.. outPathSegments])
                    };
                }

                foreach (var child in node.Children.Where(x => IsMatched(x, nextPathSegment)))
                {
                    return SearchNode(child, inPathSegments, outPathSegments, searchContext);
                }
            }

            // 404
            return new SearchResult()
            {
                EndpointContext = node.EndpointContext,
                SearchContext = searchContext,
                Uri = new UriResource([.. outPathSegments])
            };
        }

        /// <summary>
        /// Checks whether the node matches the path element.
        /// </summary>
        /// <param name="node">The sitemap node.</param>
        /// <param name="pathSegement">The path segments.</param>
        /// <returns>True if the path element matched, false otherwise.</returns>
        private static bool IsMatched(SitemapNode node, string pathSegement)
        {
            if (node == null || string.IsNullOrWhiteSpace(pathSegement))
            {
                return false;
            }

            return node.PathSegment?.IsMatched(pathSegement) ?? false;
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        private void Log()
        {
            if (!SiteMap.Any())
            {
                return;
            }

            using var frame = new LogFrameSimple(_httpServerContext.Log);
            var list = new List<string>
            {
                I18N.Translate
                (
                    "webexpress.webcore:sitemapmanager.titel"
                )
            };

            var preorder = _root
                .GetPreOrder()
                .Select(x => I18N.Translate
                (
                    "webexpress.webcore:sitemapmanager.preorder",
                    "  " + x.ToString().PadRight(60),
                    x.EndpointContext?.EndpointId.ToString() ?? ""
                ));

            foreach (var node in preorder)
            {
                list.Add(node);
            }

            _httpServerContext.Log.Info(string.Join(Environment.NewLine, list));
        }

        /// <summary>
        /// Returns a string that represents the current sitemap.
        /// </summary>
        /// <returns>A string that represents the current sitemap.</returns>
        public override string ToString()
        {
            return string.Join(" | ", _root.GetPreOrder());
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
