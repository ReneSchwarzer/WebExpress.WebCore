using System;
using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Represents the default "All" group to which every identity automatically belongs.
    /// </summary>
    public class IdentityGroupAll : IIdentityGroup
    {
        /// <summary>
        /// Returns the id of the group.
        /// </summary>
        public Guid Id { get; } = Guid.Empty;

        /// <summary>
        /// Returns the name of the group.
        /// </summary>
        public string Name => "All";

        /// <summary>
        /// Returns the policies associated with the group.
        /// </summary>
        public IEnumerable<string> Policies { get; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="policies">The policies to associate with the group.</param>
        internal IdentityGroupAll(IEnumerable<string> policies)
        {
            Policies = policies?.ToList() ?? [];
        }
    }
}
