using System;
using WebExpress.WebCondition;

namespace WebExpress.WebAttribute
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
