using System;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.WebAttribute
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
