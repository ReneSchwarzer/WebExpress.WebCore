using System;
using WebExpress.Core.WebResource;

namespace WebExpress.Core.WebAttribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParentAttribute<T> : Attribute, IResourceAttribute where T : class, IResource
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParentAttribute()
        {

        }
    }
}
