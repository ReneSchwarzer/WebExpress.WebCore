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

        /// <summary>
        /// Registers an identity provider for use within the application context.
        /// </summary>
        /// <param name="identityProvider">
        /// The identity provider to register. Cannot be null.
        /// </param>
        /// <param name="applicationContext">
        /// The application context in which the identity provider will be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if identityProvider or applicationContext is null.
        /// </exception>
        void RegisterIdentityProvider(IIdentityProvider identityProvider, IApplicationContext applicationContext);

        /// <summary>
        /// Unregisters a previously registered identity provider from the given application context.
        /// </summary>
        /// <param name="identityProvider">
        /// The identity provider to unregister. Cannot be null.
        /// </param>
        /// <param name="applicationContext">
        /// The application context from which the identity provider will be removed.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if identityProvider or applicationContext is null.
        /// </exception>
        /// <returns>
        /// True if the provider was successfully removed; false if it was not registered.
        /// </returns>
        bool UnregisterIdentityProvider(IIdentityProvider identityProvider, IApplicationContext applicationContext);

        /// <summary>
        /// Retrieves all available identities from the configured identity providers for the specified application
        /// context.
        /// </summary>
        /// <param name="applicationContext">
        /// The application context used to determine which identity providers to query. Cannot be null.
        /// </param>
        /// <returns>
        /// An enumerable collection of identities provided by all configured identity providers. The 
        /// collection is empty if no identities are available.
        /// </returns>
        IEnumerable<IIdentity> GetIdentities(IApplicationContext applicationContext);

        /// <summary>
        /// Retrieves all identity groups available from the configured group providers for the specified application
        /// context.
        /// </summary>
        /// <param name="applicationContext">
        /// The application context that determines which group providers are queried. Cannot be null.
        /// </param>
        /// <returns>
        /// An enumerable collection of identity groups available in the given application context. The 
        /// collection is empty if no groups are found.
        /// </returns>
        IEnumerable<IIdentityGroup> GetGroups(IApplicationContext applicationContext);
    }
}
