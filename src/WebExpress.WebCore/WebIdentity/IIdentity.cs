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
        /// Gets the id of the identity.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the name of the identity.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the email of the identity.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets the hash of the password.
        /// </summary>
        string PasswordHash { get; }

        /// <summary>
        /// Gets the groups associated with the identity.
        /// </summary>
        IEnumerable<IIdentityGroup> Groups { get; }
    }
}
