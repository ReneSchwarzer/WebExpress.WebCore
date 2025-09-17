using System;
using WebExpress.WebCore.WebScope;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies the scope type associated with a class. This attribute can be applied to classes 
    /// to define their scope, allowing for contextual behavior or configuration based on the 
    /// specified scope.
    /// </summary>
    /// <typeparam name="TScope">
    /// The type of the scope. Must be a class that implements the <see cref="IScope"/> interface.
    /// </typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ScopeAttribute<TScope> : Attribute, IPageAttribute, ISettingPageAttribute
        where TScope : class, IScope
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScopeAttribute()
        {

        }
    }
}
