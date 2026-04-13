using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.Test.Data
{
    /// <summary>
    /// Provides a mock implementation of the IIdentityProvider interface for testing purposes.
    /// </summary>
    /// <remarks>
    /// This class simulates an identity provider by allowing test code to manage in-memory
    /// collections of identities and groups. It is intended for use in unit tests or development scenarios where a real
    /// identity provider is not required.
    /// </remarks>
    public class MockIdentityProvider : IIdentityProvider
    {
        /// <summary>
        /// Returns the collection of identities associated with the current principal.
        /// </summary>
        public List<IIdentity> Identities { get; } = [];

        /// <summary>
        /// Returns the collection of identity groups associated with the current user or entity.
        /// </summary>
        public List<IIdentityGroup> Groups { get; } = [];

        /// <summary>
        /// Returns a collection of all associated identities for the current principal.
        /// </summary>
        /// <returns>
        /// An enumerable collection of <see cref="IIdentity"/> objects representing the identities associated with the
        /// principal. The collection may be empty if no identities are present.
        /// </returns>
        public IEnumerable<IIdentity> GetIdentities() => Identities;

        /// <summary>
        /// Retrieves a collection of identity groups associated with the current context.
        /// </summary>
        /// <returns>
        /// An enumerable collection of objects that implement the IIdentityGroup interface. The collection may be empty
        /// if no groups are associated.
        /// </returns>
        public IEnumerable<IIdentityGroup> GetGroups() => Groups;

        /// <summary>
        /// Validates the specified credentials against the identity provider.
        /// </summary>
        /// <param name="identity">The identity to validate. Cannot be null.</param>
        /// <param name="password">The password associated with the identity. Cannot be null or empty.</param>
        /// <returns>true if the credentials are valid; otherwise, false.</returns>
        public bool ValidateCredentials(IIdentity identity, string password)
            => false; // not needed for this test
    }
}
