using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebRestApi.Model
{
    /// <summary>
    /// A rest api resource that contains meta information about a rest api resource.
    /// </summary>
    internal class RestApiItem : IDisposable
    {
        private readonly IRestApiManager _restApiManager;

        /// <summary>
        /// Returns or sets the parent type.
        /// </summary>
        public Type ParentType { get; set; }

        /// <summary>
        /// Returns or sets the type of rest api resource.
        /// </summary>
        public Type RestApiClass { get; set; }

        /// <summary>
        /// Returns or sets the instance of the rest api resource, if the rest api resource is cached, otherwise null.
        /// </summary>
        public IRestApi Instance { get; set; }

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
        /// Returns the conditions that must be met for the rest api resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; set; }

        /// <summary>
        /// Returns the crud methods.
        /// </summary>
        public IEnumerable<CrudMethod> Methods { get; set; }

        /// <summary>
        /// Returns the version number of the rest api.
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache { get; set; }

        /// <summary>
        /// Returns whether it is a optional rest api resource.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Returns the log to write status messages to the console and to a log file.
        /// </summary>
        public ILog Log { get; internal set; }

        /// <summary>
        /// Returns the rest api contexts.
        /// </summary>
        public IRestApiContext RestApiContext { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiItem"/> class.
        /// </summary>
        /// <param name="restApiManager">The rest api manager.</param>
        internal RestApiItem(IRestApiManager restApiManager)
        {
            _restApiManager = restApiManager;
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
