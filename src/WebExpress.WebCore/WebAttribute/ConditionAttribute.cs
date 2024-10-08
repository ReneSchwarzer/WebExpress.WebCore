using System;
using WebExpress.WebCore.WebCondition;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Activation of options (e.g. WebEx.WebApp.Setting.SystemInformation for displaying system information).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConditionAttribute<T> : Attribute, IEndpointAttribute where T : class, ICondition
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConditionAttribute()
        {

        }
    }
}
