using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebIdentity;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebFragment
{
    /// <summary>
    /// Interface representing the context of a web fragment.
    /// Provides access to plugin context, application context, conditions for activation, and caching behavior.
    /// </summary>
    public interface IFragmentContext : IContext
    {
        /// <summary>
        /// Returns the context of the associated plugin.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Gets the unique identifier for the fragment.
        /// </summary>
        IComponentId FragmentId { get; }

        /// <summary>
        /// Returns the conditions that must be met for the component to be active.
        /// </summary>
        IEnumerable<ICondition> Conditions { get; }

        /// <summary>
        /// Returns the collection of identity policies for the endpoint.
        /// </summary>
        IEnumerable<IIdentityPolicy> Policies { get; }

        /// <summary>
        /// Returns the section.
        /// </summary>
        Type Section { get; }

        /// <summary>
        /// Returns the scope.
        /// </summary>
        Type Scope { get; }

        /// <summary>
        /// Determines whether the component is created once and reused on each execution.
        /// </summary>
        bool Cache { get; }
    }
}
