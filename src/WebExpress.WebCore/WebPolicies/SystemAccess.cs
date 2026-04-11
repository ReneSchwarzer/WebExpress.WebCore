using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.WebPolicies
{
    /// <summary>
    /// Standard policy for system-level operations such as installing, updating, and maintaining the application.
    /// </summary>
    [Name("webexpress.webcore:identitymanager.policy.systemaccess.name")]
    [Description("webexpress.webcore:identitymanager.policy.systemaccess.description")]
    public sealed class SystemAccess : IIdentityPolicy
    {
        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
