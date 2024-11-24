using System;
using System.Collections.Generic;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Represents an identity in the web application.
    /// </summary>
    public interface IIdentity
    {
        /// <summary>
        /// Returns the id of the identity.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Returns the name of the identity.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns the email of the identity.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Returns the hash of the password.
        /// </summary>
        string PasswordHash { get; }

        /// <summary>
        /// Returns the groups associated with the identity.
        /// </summary>
        IEnumerable<IIdentityGroup> Groups { get; }
    }
}
