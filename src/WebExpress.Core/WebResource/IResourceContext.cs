using System.Collections.Generic;
using WebExpress.Core.WebApplication;
using WebExpress.Core.WebCondition;
using WebExpress.Core.WebModule;
using WebExpress.Core.WebPlugin;
using WebExpress.Core.WebUri;

namespace WebExpress.Core.WebResource
{
    public interface IResourceContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the associated application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

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
        /// Returns the resource id.
        /// </summary>
        string ResourceId { get; }

        /// <summary>
        /// Returns the resource title.
        /// </summary>
        string ResourceTitle { get; }

        /// <summary>
        /// Returns the parent or null if not used.
        /// </summary>
        IResourceContext ParentContext { get; }

        /// <summary>
        /// Determines whether the resource is created once and reused each time it is called.
        /// </summary>
        bool Cache { get; }

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
