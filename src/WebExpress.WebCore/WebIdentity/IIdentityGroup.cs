using System;
using System.Collections.Generic;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Interface that defines an identity group.
    /// </summary>
    public interface IIdentityGroup
    {
        /// <summary>
        /// Gets the id of the group.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the policies associated with the group.
        /// </summary>
        IEnumerable<string> Policies { get; }
    }
}
