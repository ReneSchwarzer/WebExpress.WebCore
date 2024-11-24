using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy permission.
    /// </summary>
    [Name("Read")]
    [Description("Permissions to read.")]
    [Role<TestIdentityRoleA>()]
    public sealed class TestIdentityPermissionA : IIdentityPermission
    {
        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
