using System;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Defines the context for a policy, providing access to various related contexts and properties.
    /// </summary>
    public interface IIdentityPolicyContext : IContext
    {
        /// <summary>
        /// Returns the policy id.
        /// </summary>
        IComponentId PolicyId { get; }

        /// <summary>
        /// Returns the policy type.
        /// </summary>
        Type Policy { get; }

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }
    }
}
