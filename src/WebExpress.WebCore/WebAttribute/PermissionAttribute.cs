using System;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Connects roles with permissions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PermissionAttribute<T> : Attribute, IPermissionAttribute where T : class, IIdentityPermission
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PermissionAttribute()
        {

        }
    }
}
