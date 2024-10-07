using System.Collections.Generic;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebComponent
{
    /// <summary>
    /// Represents the context of a endpoint.
    /// </summary>
    public interface IEndpointContext : IContext
    {
        /// <summary>
        /// Returns the endpoint id.
        /// </summary>
        string EndpointId { get; }

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the corresponding module context.
        /// </summary>
        IModuleContext ModuleContext { get; }

        /// <summary>
        /// Returns the scope names that provides the resource. The scope name
        /// is a string with a name (e.g. global, admin), which can be used by elements to 
        /// determine whether content and how content should be displayed.
        /// </summary>
        IEnumerable<string> Scopes { get; }

        /// <summary>
        /// Provides the conditions that must be met for the resource to be active.
        /// </summary>
        IEnumerable<ICondition> Conditions { get; }

        /// <summary>
        /// Returns the parent or null if not used.
        /// </summary>
        IEndpointContext ParentContext { get; }

        /// <summary>
        /// Determines whether the resource is created once and reused each time it is called.
        /// </summary>
        bool Cache { get; }

        /// <summary>
        /// Returns or sets whether all subpaths should be taken into sitemap.
        /// </summary>
        bool IncludeSubPaths { get; }

        /// <summary>
        /// Returns the context path.
        /// </summary>
        UriResource ContextPath { get; }

        /// <summary>
        /// Returns the uri.
        /// </summary>
        UriResource Uri { get; }
    }
}
