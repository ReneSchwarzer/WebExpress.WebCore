using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy role.
    /// </summary>
    [Name("Editor")]
    [Description("Has permissions to create and edit, but not delete.")]
    [Permission<TestIdentityPermissionA>()]
    [Permission<TestIdentityPermissionB>()]
    public sealed class TestIdentityPolicyB : IIdentityPolicy
    {
        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
