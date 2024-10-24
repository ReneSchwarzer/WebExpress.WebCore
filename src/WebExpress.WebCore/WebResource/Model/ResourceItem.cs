using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebResource.Model
{
    /// <summary>
    /// A resource element that contains meta information about a resource.
    /// </summary>
    internal class ResourceItem : IDisposable
    {
        private readonly IResourceManager _resourceManager;

        /// <summary>
        /// Returns or sets the parent type.
        /// </summary>
        public Type ParentType { get; set; }

        /// <summary>
        /// Returns or sets the type of resource.
        /// </summary>
        public Type ResourceClass { get; set; }

        /// <summary>
        /// Returns or sets the instance of the resource, if the resource is cached, otherwise null.
        /// </summary>
        public IEndpoint Instance { get; set; }

        /// <summary>
        /// Returns the scope names that provides the resource. The scope name
        /// is a string with a name (e.g. global, admin), which can be used by elements to 
        /// determine whether content and how content should be displayed.
        /// </summary>
        //public IReadOnlyList<string> Scopes { get; set; }

        /// <summary>
        /// Returns or sets the paths of the resource.
        /// </summary>
        public UriResource ContextPath { get; set; }

        /// <summary>
        /// Returns or sets the path segment.
        /// </summary>
        public IUriPathSegment PathSegment { get; internal set; }

        /// <summary>
        /// Returns or sets whether all subpaths should be taken into sitemap.
        /// </summary>
        public bool IncludeSubPaths { get; set; }

        /// <summary>
        /// Returns the conditions that must be met for the resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; set; }

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache { get; set; }

        /// <summary>
        /// Returns the log to write status messages to the console and to a log file.
        /// </summary>
        public ILog Log { get; internal set; }

        /// <summary>
        /// Returns the resource context.
        /// </summary>
        public IResourceContext ResourceContext { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceItem"/> class.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        internal ResourceItem(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Convert the resource element to a string.
        /// </summary>
        /// <returns>The resource element in its string representation.</returns>
        public override string ToString()
        {
            return $"Resource '{ResourceContext?.EndpointId}'";
        }
    }
}
