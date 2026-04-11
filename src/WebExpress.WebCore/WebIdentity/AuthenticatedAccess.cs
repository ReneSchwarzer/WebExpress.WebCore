using WebExpress.WebCore.WebAttribute;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Standard policy for general access by authenticated users.
    /// </summary>
    [Name("webexpress.webcore:identitymanager.policy.authenticatedaccess.name")]
    [Description("webexpress.webcore:identitymanager.policy.authenticatedaccess.description")]
    public sealed class AuthenticatedAccess : IIdentityPolicy
    {
        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
