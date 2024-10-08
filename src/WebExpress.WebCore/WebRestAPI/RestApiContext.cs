using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebRestApi.Model;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// Represents the context of a rest api resource.
    /// </summary>
    public class RestApiContext : IRestApiContext
    {
        private readonly IRestApiManager _restApiManager;
        private readonly RestApiItem _restApiItem;

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; private set; }

        /// <summary>
        /// Returns the corresponding module context.
        /// </summary>
        public IModuleContext ModuleContext { get; private set; }

        /// <summary>
        /// Returns the scope names that provides the resource. The scope name
        /// is a string with a name (e.g. global, admin), which can be used by elements to 
        /// determine whether content and how content should be displayed.
        /// </summary>
        public IEnumerable<string> Scopes { get; internal set; }

        /// <summary>
        /// Returns the conditions that must be met for the resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; internal set; } = [];

        /// <summary>
        /// Returns the crud methods.
        /// </summary>
        public IEnumerable<CrudMethod> Methods { get; internal set; } = [];

        /// <summary>
        /// Returns the endpoint id.
        /// </summary>
        public string EndpointId { get; internal set; }

        /// <summary>
        /// Returns the parent or null if not used.
        /// </summary>
        public IEndpointContext ParentContext => _restApiManager.RestApis
            .Where(x => !string.IsNullOrWhiteSpace(_restApiItem.ParentId))
            .Where(x => x.EndpointId.Equals(_restApiItem.ParentId, StringComparison.OrdinalIgnoreCase))
            .Where(x => x.ModuleContext.ApplicationContext == ModuleContext.ApplicationContext)
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
                    return UriResource.Combine(ParentContext?.Uri, _restApiItem.ContextPath);
                }

                return UriResource.Combine(ModuleContext.ContextPath, _restApiItem.ContextPath);
            }
        }

        /// <summary>
        /// Returns the uri.
        /// </summary>
        public UriResource Uri => ContextPath.Append(_restApiItem.PathSegment);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        /// <param name="restApiManager">The resource manager.</param>
        /// <param name="restApiItem">The page item or null.</param>
        internal RestApiContext(IModuleContext moduleContext, IRestApiManager restApiManager, RestApiItem restApiItem = null)
        {
            PluginContext = moduleContext?.PluginContext;
            ModuleContext = moduleContext;
            _restApiManager = restApiManager;
            _restApiItem = restApiItem;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{ModuleContext?.ApplicationContext?.ApplicationId}:{ModuleContext?.ModuleId}:{EndpointId}";
        }
    }
}
