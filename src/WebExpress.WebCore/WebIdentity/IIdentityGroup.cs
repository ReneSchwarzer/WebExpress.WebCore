using System.Collections.Generic;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Interface that defines an identity group.
    /// </summary>
    public interface IIdentityGroup
    {
        /// <summary>
        /// Returns the policies associated with the group.
        /// </summary>
        IEnumerable<string> Policies { get; }
    }
}
