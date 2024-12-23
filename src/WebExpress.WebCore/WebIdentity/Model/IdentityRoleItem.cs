using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebIdentity.Model
{
    /// <summary>
    /// Represents an item in the identity role.
    /// </summary>
    public class IdentityRoleItem
    {
        private readonly IComponentHub _componentHub;

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; private set; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; private set; }

        /// <summary>
        /// Returns or sets the role context.
        /// </summary>
        public IIdentityRoleContext RoleContext { get; private set; }

        /// <summary>
        /// Returns the permissions associated with the role.
        /// </summary>
        public IEnumerable<Type> Permissions { get; private set; }

        /// <summary>
        /// Returns or sets the role class.
        /// </summary>
        public Type RoleClass { get; private set; }

        /// <summary>
        /// Returns the role instance.
        /// </summary>
        public IIdentityRole Instance { get; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The associated component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        /// <param name="pluginContext">The associated plugin context.</param>
        /// <param name="applicationContext">The corresponding application context.</param>
        /// <param name="permissionClass">The role class.</param>
        /// <param name="roleContext">The role context.</param>
        /// <param name="permissions">The permissions associated with the role.</param>
        public IdentityRoleItem(IComponentHub componentHub, IHttpServerContext httpServerContext, IPluginContext pluginContext, IApplicationContext applicationContext, Type permissionClass, IIdentityRoleContext roleContext, IEnumerable<Type> permissions)
        {
            _componentHub = componentHub;
            PluginContext = pluginContext;
            ApplicationContext = applicationContext;
            Permissions = permissions;
            RoleClass = permissionClass;
            RoleContext = roleContext;
            Instance = ComponentActivator.CreateInstance<IIdentityRole>(httpServerContext, _componentHub, RoleClass, RoleContext);
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Instance is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Convert the resource element to a string.
        /// </summary>
        /// <returns>The event element in its string representation.</returns>
        public override string ToString()
        {
            return $"Role: '{RoleClass.FullName.ToLower()}'";
        }
    }
}
