using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy permission.
    /// </summary>
    [Name("Write")]
    [Description("Permissions to write.")]
    [Role<TestIdentityRoleA>()]
    public sealed class TestIdentityPermissionB : IIdentityPermission
    {
        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
