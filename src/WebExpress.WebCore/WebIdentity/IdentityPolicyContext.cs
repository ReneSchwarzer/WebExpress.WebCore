using System;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Defines the context for a policy, providing access to various related contexts and properties.
    /// </summary>
    public class IdentityPolicyContext : IIdentityPolicyContext
    {
        /// <summary>
        /// Returns the policy id.
        /// </summary>
        public IComponentId PolicyId { get; internal set; }

        /// <summary>
        /// Returns the policy type.
        /// </summary>
        public Type Policy { get; internal set; }

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Policy: {PolicyId}";
        }
    }
}
