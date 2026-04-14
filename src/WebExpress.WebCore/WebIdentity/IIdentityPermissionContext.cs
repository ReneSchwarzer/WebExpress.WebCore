using System;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Defines the context for a permission, providing access to various related contexts and properties.
    /// </summary>
    public interface IIdentityPermissionContext : IContext
    {
        /// <summary>
        /// Gets the permission id.
        /// </summary>
        IComponentId PermissionId { get; }

        /// <summary>
        /// Gets the permission.
        /// </summary>
        Type Permission { get; }

        /// <summary>
        /// Gets the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Gets the corresponding application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }
    }
}
