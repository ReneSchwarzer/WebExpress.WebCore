using System;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebAttribute
{
    /// Specifies the status code for a starus page.
    /// </summary>
    /// <typeparam name="T">The type of the response.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StatusResponseAttribute<T> : Attribute, IStatusPageAttribute where T : Response, new()
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public StatusResponseAttribute()
        {

        }
    }
}
