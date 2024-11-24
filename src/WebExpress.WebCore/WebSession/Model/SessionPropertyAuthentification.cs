
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.WebSession.Model
{
    /// <summary>
    /// Represents the authentication session property.
    /// Authentication is the process of verifying the identity of a person or system to ensure that someone is who they claim to be.
    /// </summary>
    public class SessionPropertyAuthentification : SessionProperty
    {
        /// <summary>
        /// Returns the identity.
        /// </summary>
        public IIdentity Identity { get; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="identity">The identity to be set.</param>
        public SessionPropertyAuthentification(IIdentity identity)
        {
            Identity = identity;
        }

    }
}
