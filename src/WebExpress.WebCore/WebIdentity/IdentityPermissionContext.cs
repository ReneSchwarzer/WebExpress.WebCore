using System;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Defines the context for a permission, providing access to various related contexts and properties.
    /// </summary>
    public class IdentityPermissionContext : IIdentityPermissionContext
    {
        /// <summary>
        /// Gets the permission id.
        /// </summary>
        public IComponentId PermissionId { get; internal set; }

        /// <summary>
        /// Gets the permission.
        /// </summary>
        public Type Permission { get; internal set; }

        /// <summary>
        /// Gets the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Gets the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Permission: {PermissionId}";
        }
    }
}
