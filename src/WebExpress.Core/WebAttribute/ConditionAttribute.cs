using System;
using WebExpress.Core.WebCondition;

namespace WebExpress.Core.WebAttribute
{
    /// <summary>
    /// Activation of options (e.g. WebEx.WebApp.Setting.SystemInformation for displaying system information).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConditionAttribute<T> : Attribute, IResourceAttribute where T : class, ICondition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionAttribute()
        {

        }
    }
}
