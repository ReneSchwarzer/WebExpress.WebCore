using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebResource
{
    /// <summary>
    /// Represents the context of a resource.
    /// </summary>
    public class ResourceContext : IResourceContext
    {
        private readonly IEndpointManager _endpointManager;
        private readonly Type _parentType;
        private readonly UriResource _contextPath;
        private readonly IUriPathSegment _pathSegment;

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns the conditions that must be met for the resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; internal set; } = [];

        /// <summary>
        /// Returns the resource id.
        /// </summary>
        public string EndpointId { get; internal set; }

        /// <summary>
        /// Returns the parent or null if not used.
        /// </summary>
        public IEndpointContext ParentContext => _endpointManager.GetEndpoints(_parentType, ApplicationContext)
            .FirstOrDefault();

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache { get; internal set; }

        /// <summary>
        /// Returns or sets whether all subpaths should be taken into sitemap.
        /// </summary>
        public bool IncludeSubPaths { get; internal set; }

        /// <summary>
        /// Returns the context path.
        /// </summary>
        public UriResource ContextPath
        {
            get
            {
                var parentContext = ParentContext;
                if (parentContext != null)
                {
                    return UriResource.Combine(ParentContext?.Uri, _contextPath);
                }

                return UriResource.Combine(ApplicationContext.ContextPath, _contextPath);
            }
        }

        /// <summary>
        /// Returns the uri.
        /// </summary>
        public UriResource Uri => ContextPath.Append(_pathSegment);

        /// <summary>
        /// Initializes a new instance of the class with the specified endpoint manager, parent type, context path, and path segment.
        /// </summary>
        /// <param name="endpointManager">The endpoint manager responsible for managing endpoints.</param>
        /// <param name="parentType">The type of the parent resource.</param>
        /// <param name="contextPath">The context path of the resource.</param>
        /// <param name="pathSegment">The path segment of the resource.</param>
        public ResourceContext(IEndpointManager endpointManager, Type parentType, UriResource contextPath, IUriPathSegment pathSegment)
        {
            _endpointManager = endpointManager;
            _parentType = parentType;
            _contextPath = contextPath;
            _pathSegment = pathSegment;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Resource: {EndpointId}";
        }
    }
}
