using System;
using System.Collections.Generic;
using System.Reflection;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebRestApi.Model
{
    /// <summary>
    /// A rest api resource that contains meta information about a rest api resource.
    /// </summary>
    internal class RestApiItem : IDisposable
    {
        /// <summary>
        /// Returns the endpoint id.
        /// </summary>
        public IComponentId EndpointId { get; internal set; }

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns or sets the type of rest api resource.
        /// </summary>
        public Type RestApiClass { get; internal set; }

        /// <summary>
        /// Returns or sets the instance of the rest api resource, if the rest api resource is cached, otherwise null.
        /// </summary>
        public IRestApi Instance { get; internal set; }

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
        /// Returns the conditions that must be met for the rest api resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; internal set; }

        /// <summary>
        /// Returns the crud methods.
        /// </summary>
        public IEnumerable<RequestMethod> Methods { get; internal set; }

        /// <summary>
        /// Returns the version number of the rest api.
        /// </summary>
        public uint Version { get; internal set; }

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache { get; internal set; }

        /// <summary>
        /// Returns the reflection information for the associated get method.
        /// </summary>
        public MethodInfo GetMethod { get; internal set; }

        /// <summary>
        /// Returns the reflection information for the associated post method.
        /// </summary>
        public MethodInfo PostMethod { get; internal set; }

        /// <summary>
        /// Returns the reflection information for the associated patch method.
        /// </summary>
        public MethodInfo PatchMethod { get; internal set; }

        /// <summary>
        /// Returns the reflection information for the associated put method.
        /// </summary>
        public MethodInfo PutMethod { get; internal set; }

        /// <summary>
        /// Returns the reflection information for the associated delete method.
        /// </summary>
        public MethodInfo DeleteMethod { get; internal set; }

        /// <summary>
        /// Returns the attributes associated with the page.
        /// </summary>
        public IEnumerable<Type> Attributes { get; internal set; }

        /// <summary>
        /// Returns the rest api contexts.
        /// </summary>
        public IRestApiContext RestApiContext { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiItem"/> class.
        /// </summary>
        /// <param name="endpointManager">The endpoint manager responsible for managing endpoints.</param>
        internal RestApiItem(IEndpointManager endpointManager)
        {
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged rest api resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Convert the resource element to a string.
        /// </summary>
        /// <returns>The rest api resource element in its string representation.</returns>
        public override string ToString()
        {
            return $"RestApi: '{RestApiContext?.EndpointId}'";
        }
    }
}
