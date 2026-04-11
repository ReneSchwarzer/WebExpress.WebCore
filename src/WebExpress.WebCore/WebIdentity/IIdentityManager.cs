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
        /// Returns all policies.
        /// </summary>
        public IEnumerable<IIdentityPolicyContext> Policies { get; }

        /// <summary>
        /// Returns all identities.
        /// </summary>
        IEnumerable<IIdentity> Identities { get; }

        /// <summary>
        /// Returns the default "All" group to which every identity automatically belongs.
        /// </summary>
        IdentityGroupAll AllGroup { get; }

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
        bool Login(IRequest request, IIdentity identity, SecureString password);

        /// <summary>
        /// Logout an identity.
        /// </summary>
        /// <param name="request">The request.</param>
        void Logout(IRequest request);

        /// <summary>
        /// Returns the current signed-in identity based on the provided request.
        /// </summary>
        /// <param name="request">The request to get the current identity for.</param>
        /// <returns>The current signed-in identity.</returns>
        IIdentity GetCurrentIdentity(IRequest request);

        /// <summary>
        /// Checks if the specified identity has the given permission.
        /// </summary>
        /// <typeparam name="TIdentityPermission">The type of the identity permission.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="identity">The identity to check.</param>
        /// <returns>True if the identity has the permission, false otherwise.</returns>
        bool CheckAccess<TIdentityPermission>(IApplicationContext applicationContext, IIdentity identity)
            where TIdentityPermission : IIdentityPermission;

        /// <summary>
        /// Checks whether the given identity has the specified permission by evaluating all associated groups.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="identity">The identity to check.</param>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>True if any group grants the permission, false otherwise.</returns>
        bool CheckAccess(IApplicationContext applicationContext, IIdentity identity, Type permission);

        /// <summary>
        /// Checks if the specified identity group has the given permission.
        /// </summary>
        /// <typeparam name="TIdentityPermission">The type of the identity permission.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="group">The identity group to check.</param>
        /// <returns>True if the identity group has the permission, false otherwise.</returns>
        bool CheckAccess<TIdentityPermission>(IApplicationContext applicationContext, IIdentityGroup group)
            where TIdentityPermission : IIdentityPermission;

        /// <summary>
        /// Checks if the specified identity group has the given permission.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="group">The identity group to check.</param>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>True if the identity group has the permission, false otherwise.</returns>
        bool CheckAccess(IApplicationContext applicationContext, IIdentityGroup group, Type permission);

        /// <summary>
        /// Checks if the specified identity policy has the given permission.
        /// </summary>
        /// <typeparam name="TIdentityPolicy">The type of the identity policy.</typeparam>
        /// <typeparam name="TIdentityPermission">The type of the identity permission.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>True if the identity policy has the permission, false otherwise.</returns>
        bool CheckAccess<TIdentityPolicy, TIdentityPermission>(IApplicationContext applicationContext)
            where TIdentityPolicy : IIdentityPolicy
            where TIdentityPermission : IIdentityPermission;

        /// <summary>
        /// Checks if the specified identity policy has the given permission.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="policy">The identity policy to check.</param>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>True if the identity policy has the permission, false otherwise.</returns>
        bool CheckAccess(IApplicationContext applicationContext, Type policy, Type permission);
    }
}
