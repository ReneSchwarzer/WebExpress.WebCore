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
    /// Represents the context for a web fragment, including plugin context, application context,
    /// conditions for activation, culture information, and caching behavior.
    /// </summary>
    public class FragmentContext : IFragmentContext
    {
        /// <summary>
        /// Returns the context of the associated plugin.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Gets the unique identifier for the fragment.
        /// </summary>
        public IComponentId FragmentId { get; internal set; }

        /// <summary>
        /// Returns the conditions that must be met for the component to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; internal set; } = [];

        /// <summary>
        /// Returns the collection of identity policies for the endpoint.
        /// </summary>
        public IEnumerable<IIdentityPolicy> Policies { get; internal set; } = [];

        /// <summary>
        /// Determines whether the component is created once and reused on each execution.
        /// </summary>
        public bool Cache { get; internal set; }

        /// <summary>
        /// Returns the section.
        /// </summary>
        public Type Section { get; internal set; }

        /// <summary>
        /// Returns the scope.
        /// </summary>
        public Type Scope { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FragmentContext()
        {
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Fragment: {FragmentId}";
        }
    }
}
