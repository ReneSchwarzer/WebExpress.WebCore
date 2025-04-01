using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebAsset
{
    /// <summary>
    /// Represents the context of a asset.
    /// </summary>
    public class AssetContext : IAssetContext
    {
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
        public IEnumerable<ICondition> Conditions => [];

        /// <summary>
        /// Returns the resource id.
        /// </summary>
        public IComponentId EndpointId { get; internal set; }

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache => true;

        /// <summary>
        /// Returns or sets whether all subpaths should be taken into sitemap.
        /// </summary>
        public bool IncludeSubPaths { get; internal set; }

        /// <summary>
        /// Returns the internal routing path for the endpoint.
        /// </summary>
        public IRoute Route { get; internal set; }

        /// <summary>
        /// Returns the attributes associated with the page.
        /// </summary>
        public IEnumerable<Type> Attributes => [];

        /// <summary>
        /// Initializes a new instance of the class with the specified endpoint manager, parent type, context path, and path segment.
        /// </summary>
        public AssetContext()
        {
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Asset: {EndpointId}";
        }
    }
}
