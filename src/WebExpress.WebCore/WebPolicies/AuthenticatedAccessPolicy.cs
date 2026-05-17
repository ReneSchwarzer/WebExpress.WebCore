using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.WebPolicies
{
    /// <summary>
    /// Standard policy for general access by authenticated users.
    /// </summary>
    [Name("webexpress.webcore:identitymanager.policy.authenticatedaccess.name")]
    [Description("webexpress.webcore:identitymanager.policy.authenticatedaccess.description")]
    public sealed class AuthenticatedAccessPolicy : IIdentityPolicy
    {
        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
