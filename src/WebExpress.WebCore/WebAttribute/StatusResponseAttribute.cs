using System;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies the status code for a starus page.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StatusResponseAttribute<TResponse> : Attribute, IStatusPageAttribute
        where TResponse : Response, new()
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public StatusResponseAttribute()
        {

        }
    }
}
