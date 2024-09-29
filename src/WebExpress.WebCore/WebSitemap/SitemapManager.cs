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
using WebExpress.WebCore.WebResource;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSitemap
{
    /// <summary>
    /// The sitemap manager manages WebExpress elements, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public sealed class SitemapManager : ISitemapManager, ISystemComponent
    {
        private SitemapNode _root = new();
        private readonly IComponentManager _componentManager;
        private readonly IResourceManager _resourceManager;
        private readonly IHttpServerContext _httpServerContext;

        /// <summary>
        /// Returns the side map.
        /// </summary>
        public IEnumerable<IResourceContext> SiteMap => _root.GetPreOrder()
            .Where(x => x != null)
            .Select(x => x.ResourceContext);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentManager">The component manager.</param>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private SitemapManager(IComponentManager componentManager, IResourceManager resourceManager, IHttpServerContext httpServerContext)
        {
            _componentManager = componentManager;
            _resourceManager = resourceManager as ResourceManager;

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress:sitemapmanager.initialization")
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
            var resources = _resourceManager.Resources
                .Select(x => new
                {
                    ResourceContext = x,
                    x.Uri.PathSegments
                })
                .OrderBy(x => x.PathSegments.Count);

            foreach (var item in resources)
            {
                MergeSitemap(newSiteMapNode, CreateSiteMap
                (
                    new Queue<IUriPathSegment>(item.PathSegments),
                    item.ResourceContext
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

            if (result != null && result.ResourceContext != null)
            {
                if (!result.ResourceContext.Conditions.Any() || result.ResourceContext.Conditions.All(x => x.Fulfillment(searchContext.HttpContext?.Request)))
                {
                    return result;
                }
            }

            // 404
            return result;
        }

        /// <summary>
        /// Determines the Uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <typeparam name="T">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid>/b').</typeparam>
        /// <paramref name="parameters"/>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        public UriResource GetUri<T>(params Parameter[] parameters) where T : IResource
        {
            var resourceContexts = _resourceManager.GetResorces<T>();

            var node = _root.GetPreOrder()
                .Where(x => resourceContexts.Contains(x.ResourceContext))
                .FirstOrDefault();

            return node?.ResourceContext?.Uri.SetParameters(parameters);
        }

        /// <summary>
        /// Determines the Uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="parameters">The parameters to be considered for the URI.</param>
        /// <returns>Returns the URI taking into account the context, or null if no valid URI is found.</returns>
        public UriResource GetUri(Type resourceType, params Parameter[] parameters)
        {
            var resourceContexts = _resourceManager.GetResorces(resourceType);

            var node = _root.GetPreOrder()
                .Where(x => resourceContexts.Contains(x.ResourceContext))
                .FirstOrDefault();

            return node?.ResourceContext?.Uri.SetParameters(parameters);
        }

        /// <summary>
        /// Determines the Uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <typeparam name="T">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid>/b').</typeparam>
        /// <param name="resourceContext">The module context.</param>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        public UriResource GetUri<T>(IModuleContext moduleContext) where T : IResource
        {
            var resourceContexts = _resourceManager.GetResorces<T>(moduleContext);

            var node = _root.GetPreOrder()
                .Where(x => resourceContexts.Contains(x.ResourceContext))
                .FirstOrDefault();

            return node?.ResourceContext?.Uri;
        }

        /// <summary>
        /// Determines the Uri from the sitemap of a class, taking into account the context in which the uri is valid.
        /// </summary>
        /// <typeparam name="T">The class from which the uri is to be determined. The class uri must not have any dynamic components (such as '/a/<guid>/b').</typeparam>
        /// <param name="resourceContext">The module context.</param>
        /// <returns>Returns the uri taking into account the context or null.</returns>
        public UriResource GetUri<T>(IResourceContext resourceContext) where T : IResource
        {
            var resourceContexts = _resourceManager.GetResorces<T>(resourceContext.ModuleContext)
                .Where(x => x.ResourceId.Equals(resourceContext.ResourceId, StringComparison.OrdinalIgnoreCase));

            var node = _root.GetPreOrder()
                .Where(x => resourceContexts.Contains(x.ResourceContext))
                .FirstOrDefault();

            return node?.ResourceContext?.Uri;
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
            var pathSegment = contextPathSegments.Any() ? contextPathSegments.Dequeue() : null;

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
        /// <param name="resourceContext">The resource context.</param>
        /// <returns>The sitemap root node.</returns>
        private static SitemapNode CreateSiteMap
        (
            Queue<IUriPathSegment> contextPathSegments,
            IResourceContext resourceContext
        )
        {
            var root = new SitemapNode() { PathSegment = new UriPathSegmentRoot() };
            var next = CreateSiteMap(contextPathSegments, resourceContext, root);

            if (next != null)
            {
                root.Children.Add(next);
            }

            return root;
        }

        /// <summary>
        /// Creates the sitemap. Works recursively.
        /// It is important for the algorithm that the addition of resources is sorted 
        /// by the number of path segments in ascending order.
        /// </summary>
        /// <param name="contextPathSegments">The path segments of the context path.</param>
        /// <param name="resourceContext">The resource context.</param>
        /// <param name="parent">The parent node or null if root.</param>
        /// <returns>The sitemap parent node.</returns>
        private static SitemapNode CreateSiteMap
        (
            Queue<IUriPathSegment> contextPathSegments,
            IResourceContext resourceContext,
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
                ResourceContext = resourceContext
            };

            if (contextPathSegments.Any())
            {
                node.Children.Add(CreateSiteMap(contextPathSegments, resourceContext, node));
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
                        if (fc.ResourceContext == null)
                        {
                            fc.ResourceContext = sc.ResourceContext;
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
            var pathSegment = inPathSegments.Any() ? inPathSegments.Dequeue() : null;
            var nextPathSegment = inPathSegments.Any() ? inPathSegments.Peek() : null;

            if (IsMatched(node, pathSegment))
            {
                var copy = node.PathSegment.Copy();
                if (copy is UriPathSegmentVariable variable)
                {
                    variable.Value = pathSegment;
                }

                outPathSegments.Enqueue(copy);

                if (nextPathSegment == null)
                {
                    return new SearchResult()
                    {
                        ResourceId = node.ResourceContext.ResourceId,
                        ResourceContext = node.ResourceContext,
                        SearchContext = searchContext,
                        Uri = new UriResource(outPathSegments.ToArray()),
                        Instance = CreateInstance(node, new UriResource(outPathSegments.ToArray()), searchContext),
                    };
                }
                else if (node.IsLeaf && nextPathSegment != null && node.ResourceContext != null && node.ResourceContext.IncludeSubPaths)
                {
                    return new SearchResult()
                    {
                        ResourceId = node.ResourceContext.ResourceId,
                        ResourceContext = node.ResourceContext,
                        SearchContext = searchContext,
                        Uri = new UriResource(outPathSegments.ToArray()),
                        Instance = CreateInstance(node, new UriResource(outPathSegments.ToArray()), searchContext),
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
                ResourceContext = node.ResourceContext,
                SearchContext = searchContext,
                Uri = new UriResource(outPathSegments.ToArray())
            };
        }

        /// <summary>
        /// Creates a new instance or if caching is active, a possibly existing instance is returned.
        /// </summary>
        /// <param name="node">The sitemap node.</param>
        /// <param name="uri">The uri.</param>
        /// <param name="context">The search context.</param>
        /// <returns>The instance or null.</returns>
        private IResource CreateInstance(SitemapNode node, UriResource uri, SearchContext context)
        {
            if (node == null || node.ResourceContext == null)
            {
                return null;
            }

            var instance = _resourceManager.CreateResourceInstance(node.ResourceContext, context.Culture);

            //if (instance is IPage page)
            //{
            //}

            return instance;
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
                    x.ResourceContext?.ResourceId ?? ""
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
