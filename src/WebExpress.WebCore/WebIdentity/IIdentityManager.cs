using System;
using System.Collections.Generic;
using System.Security;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Interface for managing identities.
    /// </summary>
    public interface IIdentityManager : IComponentManager
    {
        /// <summary>
        /// Returns all permissions.
        /// </summary>
        public IEnumerable<IIdentityPermissionContext> Permissions { get; }

        /// <summary>
        /// Returns all roles.
        /// </summary>
        public IEnumerable<IIdentityRoleContext> Roles { get; }

        /// <summary>
        /// Returns all identities.
        /// </summary>
        IEnumerable<IIdentity> Identities { get; }

        /// <summary>
        /// Returns the current signed-in identity.
        /// </summary>
        IIdentity CurrentIdentity { get; }

        /// <summary>
        /// Login an identity.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="identity">The identity.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if successful, false otherwise.</returns>
        bool Login(Request request, IIdentity identity, SecureString password);

        /// <summary>
        /// Logout an identity.
        /// </summary>
        /// <param name="request">The request.</param>
        void Logout(Request request);

        /// <summary>
        /// Returns the current signed-in identity based on the provided request.
        /// </summary>
        /// <param name="request">The request to get the current identity for.</param>
        /// <returns>The current signed-in identity.</returns>
        IIdentity GetCurrentIdentity(Request request);

        /// <summary>
        /// Checks if the specified identity has the given permission.
        /// </summary>
        /// <typeparam name="T">The type of the identity permission.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="identity">The identity to check.</param>
        /// <returns>True if the identity has the permission, false otherwise.</returns>
        bool CheckAccess<T>(IApplicationContext applicationContext, IIdentity identity) where T : IIdentityPermission;

        /// <summary>
        /// Checks if the specified identity has the given permission.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="identity">The identity to check.</param>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>True if the identity has the permission, false otherwise.</returns>
        bool CheckAccess(IApplicationContext applicationContext, IIdentity identity, Type permission);

        /// <summary>
        /// Checks if the specified identity group has the given permission.
        /// </summary>
        /// <typeparam name="T">The type of the identity permission.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="group">The identity group to check.</param>
        /// <returns>True if the identity group has the permission, false otherwise.</returns>
        bool CheckAccess<T>(IApplicationContext applicationContext, IIdentityGroup group) where T : IIdentityPermission;

        /// <summary>
        /// Checks if the specified identity group has the given permission.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="group">The identity group to check.</param>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>True if the identity group has the permission, false otherwise.</returns>
        bool CheckAccess(IApplicationContext applicationContext, IIdentityGroup group, Type permission);

        /// <summary>
        /// Checks if the specified identity role has the given permission.
        /// </summary>
        /// <typeparam name="R">The type of the identity role.</typeparam>
        /// <typeparam name="P">The type of the identity permission.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>True if the identity role has the permission, false otherwise.</returns>
        bool CheckAccess<R, P>(IApplicationContext applicationContext) where R : IIdentityRole where P : IIdentityPermission;

        /// <summary>
        /// Checks if the specified identity role has the given permission.
        /// </summary>
        /// <param name="role">The identity role to check.</param>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>True if the identity role has the permission, false otherwise.</returns>
        bool CheckAccess(IApplicationContext applicationContext, Type role, Type permission);
    }
}
