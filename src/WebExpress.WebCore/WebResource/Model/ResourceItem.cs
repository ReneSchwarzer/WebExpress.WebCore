using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebResource.Model
{
    /// <summary>
    /// A resource element that contains meta information about a resource.
    /// </summary>
    internal class ResourceItem : IDisposable
    {
        /// <summary>
        /// Returns the endpoint id.
        /// </summary>
        public IComponentId EndpointId { get; internal set; }

        /// <summary>
        /// Returns the context of the associated plugin.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns or sets the type of resource.
        /// </summary>
        public Type ResourceClass { get; internal set; }

        /// <summary>
        /// Returns or sets the instance of the resource, if the resource is cached, otherwise null.
        /// </summary>
        public IEndpoint Instance { get; internal set; }

        /// <summary>
        /// Returns or sets the paths of the resource.
        /// </summary>
        public UriEndpoint ContextPath { get; internal set; }

        /// <summary>
        /// Returns or sets the path segment.
        /// </summary>
        public IUriPathSegment PathSegment { get; internal set; }

        /// <summary>
        /// Returns or sets whether all subpaths should be taken into sitemap.
        /// </summary>
        public bool IncludeSubPaths { get; internal set; }

        /// <summary>
        /// Returns the conditions that must be met for the resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; internal set; }

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache { get; internal set; }

        /// <summary>
        /// Returns the attributes associated with the page.
        /// </summary>
        public IEnumerable<Type> Attributes { get; internal set; }

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
            return $"Resource: '{ResourceContext?.EndpointId}'";
        }
    }
}
