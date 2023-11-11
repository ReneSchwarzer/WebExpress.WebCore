using System;

namespace WebExpress.Core.WebAttribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StatusCodeAttribute : System.Attribute, IApplicationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="status">The status code.</param>
        public StatusCodeAttribute(int status)
        {

        }
    }
}
