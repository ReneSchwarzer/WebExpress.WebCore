using System;
using WebExpress.Core.WebScope;

namespace WebExpress.Core.WebAttribute
{
    /// <summary>
    /// The range in which the component is valid.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ScopeAttribute<T> : Attribute, IResourceAttribute where T : class, IScope
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ScopeAttribute()
        {

        }
    }
}
