using System;
using WebExpress.WebResource;

namespace WebExpress.WebAttribute
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
