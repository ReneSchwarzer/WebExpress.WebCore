using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebIdentity;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebEndpoint
{
    /// <summary>
    /// Represents the context of a endpoint.
    /// </summary>
    public interface IEndpointContext : IContext
    {
        /// <summary>
        /// Returns the endpoint id.
        /// </summary>
        IComponentId EndpointId { get; }

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Provides the conditions that must be met for the resource to be active.
        /// </summary>
        IEnumerable<ICondition> Conditions { get; }

        /// <summary>
        /// Determines whether the resource is created once and reused each time it is called.
        /// </summary>
        bool Cache { get; }

        /// <summary>
        /// Returns or sets whether all subpaths should be taken into sitemap.
        /// </summary>
        bool IncludeSubPaths { get; }

        /// <summary>
        /// Returns the internal routing path for the endpoint.
        /// </summary>
        IRoute Route { get; }

        /// <summary>
        /// Returns the attributes associated with the endpoint.
        /// </summary>
        IEnumerable<Attribute> Attributes { get; }

        /// <summary>
        /// Returns the collection of identity policies for the endpoint.
        /// </summary>
        IEnumerable<IIdentityPolicy> Policies { get; }
    }
}
