using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy role.
    /// </summary>
    [Name("Admin")]
    [Description("Has permissions to create, edit, and delete data.")]
    public sealed class TestIdentityRoleA : IIdentityRole
    {
        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
