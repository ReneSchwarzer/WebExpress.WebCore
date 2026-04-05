using System;
using WebExpress.WebCore.WebDomain;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies that the decorated class belongs to a particular domain.
    /// Domains represent logical application areas such as workspaces,
    /// modules or functional segments. Multiple domain attributes may be
    /// applied to the same class to associate it with several domains.
    /// </summary>
    /// <typeparam name="TDomain">
    /// The domain type implementing <see cref="IDomain"/> that describes
    /// the logical area to which the class belongs.
    /// </typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DomainAttribute<TDomain> : Attribute, IPageAttribute, ISettingPageAttribute
        where TDomain : class, IDomain
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DomainAttribute()
        {

        }
    }
}
