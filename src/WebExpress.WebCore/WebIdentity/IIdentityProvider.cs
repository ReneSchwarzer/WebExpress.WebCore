using System.Collections.Generic;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Represents an external identity provider that supplies identities and groups
    /// to the WebExpress identity system.
    /// </summary>
    public interface IIdentityProvider
    {
        /// <summary>
        /// Returns all identities provided by this source.
        /// </summary>
        IEnumerable<IIdentity> GetIdentities();

        /// <summary>
        /// Returns all groups provided by this source.
        /// </summary>
        IEnumerable<IIdentityGroup> GetGroups();

        /// <summary>
        /// Validates the credentials of the given identity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if the credentials are valid, false otherwise.</returns>
        bool ValidateCredentials(IIdentity identity, string password);
    }
}
