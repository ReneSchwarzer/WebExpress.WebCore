using System;

namespace WebExpress.WebCore.WebAttribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StatusCodeAttribute : System.Attribute, IApplicationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="status">The status code.</param>
        public StatusCodeAttribute(int status)
        {

        }
    }
}
