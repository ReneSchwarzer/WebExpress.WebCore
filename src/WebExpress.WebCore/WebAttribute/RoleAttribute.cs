using System;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Connects roles with permissions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RoleAttribute<T> : Attribute, IRoleAttribute where T : class, IIdentityRole
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RoleAttribute()
        {

        }
    }
}
