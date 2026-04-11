using WebExpress.WebCore.WebAttribute;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Standard policy for accessing public resources without authentication.
    /// </summary>
    [Name("webexpress.webcore:identitymanager.policy.publicaccess.name")]
    [Description("webexpress.webcore:identitymanager.policy.publicaccess.description")]
    public sealed class PublicAccess : IIdentityPolicy
    {
        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
