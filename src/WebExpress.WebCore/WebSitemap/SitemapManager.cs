using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSitemap
{
    /// <summary>
    /// The sitemap manager manages WebExpress elements, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public sealed class SitemapManager : ISitemapManager, ISystemComponent
    {
        private SitemapNode _root = new();
        private readonly Dictionary<Type, EndpointRegistration> _registrations = [];
        private readonly IComponentHub _componentManager;
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
        /// <param name="componentManager">The component manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private SitemapManager(IComponentHub componentManager, IHttpServerContext httpServerContext)
        {
            _componentManager = componentManager;
            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress:sitemapmanager.initialization")
            );
        }

        /// <summary>
        /// Registers an endpoint manager.
        /// </summary>
        /// <typeparam name="T">The type of the endpoint manager.</typeparam>
        /// <param name="registration">The registration details containing the callback functions.</param>
        public void Register<T>(EndpointRegistration registration) where T : IEndpointContext
        {
            var type = typeof(T);
            if (!_registrations.ContainsKey(type))
            {
                _registrations[type] = registration;
            }
        }

        /// <summary>
        /// Removes the registration for a specific endpoint manager.
        /// </summary>
        /// <typeparam name="T">The type of the endpoint manager.</typeparam>
        public void Remove<T>() where T : IEndpointContext
        {
            var type = typeof(T);
            _registrations.Remove(type);
        }

        /// <summary>
        /// Rebuilds the sitemap.
        /// </summary>
        public void Refresh()
        {
            var newSiteMapNode = new SitemapNode() { PathSegment = new UriPathSegmentRoot() };

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress:sitemapmanager.refresh")
            );

            // applications
            var applications = _componentManager.ApplicationManager.Applications
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

            // modules
            var modules = _componentManager.ModuleManager.Modules
                .Select(x => new
                {
                    ModuleContext = x,
                    x.ContextPath.PathSegments
                })
                .OrderBy(x => x.PathSegments.Count);

            foreach (var module in modules)
            {
                MergeSitemap(newSiteMapNode, CreateSiteMap
                (
                    new Queue<IUriPathSegment>(module.PathSegments),
                    module.ModuleContext
                ));
            }

            // resourcen
            var resources = _registrations.Values.SelectMany(x => x.EndpointResolver())
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

            using var frame = new LogFrameSimple(_httpServerContext.Log);
            var list = new List<string>();
            PrepareForLog(null, list, 2);
            _httpServerContext.Log.Info(string.Join(Environment.NewLine, list));
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
                new Queue<string>(requestUri.Segments.Select(x => x == "/" ? x : (x.EndsWith("/") ? x[..^1] : x))),
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
        /// <typeparam name="T">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid>/b').</typeparam>
        /// <paramref name="parameters"/>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        public UriResource GetUri<T>(params Parameter[] parameters) where T : IEndpoint
        {
            var endpointContexts = ResolveEndpointContexts(typeof(T));

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
            var endpointContexts = ResolveEndpointContexts(resourceType);

            var node = _root.GetPreOrder()
                .Where(x => endpointContexts.Contains(x.EndpointContext))
                .FirstOrDefault();

            return node?.EndpointContext?.Uri.SetParameters(parameters);
        }

        /// <summary>
        /// Determines the uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <typeparam name="T">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid>/b').</typeparam>
        /// <param name="resourceContext">The module context.</param>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        public UriResource GetUri<T>(IModuleContext moduleContext) where T : IEndpoint
        {
            var endpointContexts = ResolveEndpointContexts(typeof(T), moduleContext);

            var node = _root.GetPreOrder()
                .Where(x => endpointContexts.Contains(x.EndpointContext))
                .FirstOrDefault();

            return node?.EndpointContext?.Uri;
        }

        /// <summary>
        /// Determines the Uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <typeparam name="T">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid>/b').</typeparam>
        /// <param name="endpointContext">The endpoint context.</param>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        public UriResource GetUri<T>(IEndpointContext endpointContext) where T : IEndpoint
        {
            var endpointContexts = ResolveEndpointContexts(typeof(T), endpointContext.ModuleContext)
                .Where(x => x.EndpointId.Equals(endpointContext.EndpointId, StringComparison.OrdinalIgnoreCase));

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
        /// <param name="parent">The parent node or null if root.</param>
        /// <returns>The sitemap root node.</returns>
        private static SitemapNode CreateSiteMap
        (
            Queue<IUriPathSegment> contextPathSegments,
            IApplicationContext applicationContext
        )
        {
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
            var pathSegment = contextPathSegments.Any() ? contextPathSegments.Dequeue() : null;

            if (pathSegment == null)
            {
                return null;
            }

            var node = new SitemapNode()
            {
                PathSegment = pathSegment,
                Parent = parent,
            };

            if (contextPathSegments.Any())
            {
                node.Children.Add(CreateSiteMap(contextPathSegments, applicationContext, node));
            }

            return node;
        }

        /// <summary>
        /// Creates the sitemap. Works recursively.
        /// It is important for the algorithm that the addition of module is sorted 
        /// by the number of path segments in ascending order.
        /// </summary>
        /// <param name="contextPathSegments">The path segments of the context path.</param>
        /// <param name="moduleContext">The module context.</param>
        /// <returns>The sitemap root node.</returns>
        private static SitemapNode CreateSiteMap
        (
            Queue<IUriPathSegment> contextPathSegments,
            IModuleContext moduleContext
        )
        {
            var root = new SitemapNode() { PathSegment = new UriPathSegmentRoot() };
            var next = CreateSiteMap(contextPathSegments, moduleContext, root);

            if (next != null)
            {
                root.Children.Add(next);
            }

            return root;
        }

        /// <summary>
        /// Creates the sitemap. Works recursively.
        /// It is important for the algorithm that the addition of module is sorted 
        /// by the number of path segments in ascending order.
        /// </summary>
        /// <param name="contextPathSegments">The path segments of the context path.</param>
        /// <param name="moduleContext">The application context.</param>
        /// <param name="parent">The parent node or null if root.</param>
        /// <returns>The sitemap root node.</returns>
        private static SitemapNode CreateSiteMap
        (
            Queue<IUriPathSegment> contextPathSegments,
            IModuleContext moduleContext,
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
                PathSegment = pathSegment as IUriPathSegment,
                Parent = parent,
            };

            if (contextPathSegments.Any())
            {
                node.Children.Add(CreateSiteMap(contextPathSegments, moduleContext, node));
            }

            return node;
        }

        /// <summary>
        /// Creates the sitemap. Works recursively.
        /// It is important for the algorithm that the addition of module is sorted 
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
            var root = new SitemapNode() { PathSegment = new UriPathSegmentRoot() };
            var next = CreateSiteMap(contextPathSegments, endpointContext, root);

            if (next != null)
            {
                root.Children.Add(next);
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
            var pathSegment = contextPathSegments.Any() ? contextPathSegments.Dequeue() : null;

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

            if (contextPathSegments.Any())
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
                        if (fc.EndpointContext == null)
                        {
                            fc.EndpointContext = sc.EndpointContext;
                            //fc.Parent = sc.Parent;
                        }

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
        private SearchResult SearchNode
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
                var registration = default(EndpointRegistration);

                if (type != null && _registrations.TryGetValue(type, out var _registration))
                {
                    registration = _registration;
                }

                outPathSegments.Enqueue(copy);

                if (nextPathSegment == null)
                {
                    return new SearchResult(registration?.HandleRequest)
                    {
                        EndpointId = node.EndpointContext.EndpointId,
                        EndpointContext = node.EndpointContext,
                        SearchContext = searchContext,
                        Uri = new UriResource([.. outPathSegments]),
                        Instance = CreateEndpoint(node, new UriResource([.. outPathSegments]), searchContext)
                    };
                }
                else if (node.IsLeaf && nextPathSegment != null && node.EndpointContext != null && node.EndpointContext.IncludeSubPaths)
                {
                    return new SearchResult(registration?.HandleRequest)
                    {
                        EndpointId = node.EndpointContext.EndpointId,
                        EndpointContext = node.EndpointContext,
                        SearchContext = searchContext,
                        Uri = new UriResource([.. outPathSegments]),
                        Instance = CreateEndpoint(node, new UriResource([.. outPathSegments]), searchContext)
                    };
                }

                foreach (var child in node.Children.Where(x => IsMatched(x, nextPathSegment)))
                {
                    return SearchNode(child, inPathSegments, outPathSegments, searchContext);
                }
            }

            // 404
            return new SearchResult(null)
            {
                EndpointContext = node.EndpointContext,
                SearchContext = searchContext,
                Uri = new UriResource([.. outPathSegments])
            };
        }

        /// <summary>
        /// Creates a new instance or if caching is active, a possibly existing instance is returned.
        /// </summary>
        /// <param name="node">The sitemap node.</param>
        /// <param name="uri">The uri.</param>
        /// <param name="context">The search context.</param>
        /// <returns>The created endpoint.</returns>
        private IEndpoint CreateEndpoint(SitemapNode node, UriResource uri, SearchContext context)
        {
            if (node == null || node.EndpointContext == null)
            {
                return null;
            }

            var type = node.EndpointContext.GetType();

            if (_registrations.TryGetValue(type, out var registration))
            {
                return registration.Factory(node.EndpointContext, uri, context.Culture);
            }

            throw new InvalidOperationException($"No factory registered for type {type}");
        }

        /// <summary>
        /// Resolves the endpoint context for the specified endpoint.
        /// </summary>
        /// <param name="endpointType">The type of the endpoint.</param>
        /// <param name="moduleContext">The optional module context.</param>
        /// <returns>An enumerable of the corresponding endpoint contexts.</returns>
        private IEnumerable<IEndpointContext> ResolveEndpointContexts(Type endpointType, IModuleContext moduleContext = null)
        {
            foreach (var contextResolver in _registrations.Values.Select(x => x.ContextResolver))
            {
                var res = contextResolver(endpointType, moduleContext);

                if (res.Any())
                {
                    return res;
                }
            }

            return [];
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
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
            output.Add
            (
                I18N.Translate
                (
                    "webexpress:sitemapmanager.sitemap"
                )
            );

            var preorder = _root
                .GetPreOrder()
                .Select(x => I18N.Translate
                (
                    "webexpress:sitemapmanager.preorder",
                    "  " + x.ToString().PadRight(60),
                    x.EndpointContext?.EndpointId ?? ""
                ));

            foreach (var node in preorder)
            {
                output.Add(node);
            }
        }

        /// <summary>
        /// Returns a string that represents the current sitemap.
        /// </summary>
        /// <returns>A string that represents the current sitemap.</returns>
        public override string ToString()
        {
            return string.Join(" | ", _root.GetPreOrder());
        }
    }
}
