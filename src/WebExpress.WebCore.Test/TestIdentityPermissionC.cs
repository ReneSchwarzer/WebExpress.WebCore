using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy permission.
    /// </summary>
    [Name("Delte")]
    [Description("Permissions to delete.")]
    [Role<TestIdentityRoleA>()]
    public sealed class TestIdentityPermissionC : IIdentityPermission
    {
        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
