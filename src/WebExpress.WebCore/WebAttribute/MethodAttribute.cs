using System;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// The range in which the attribute is valid.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MethodAttribute : Attribute, IEndpointAttribute
    {
        /// <summary>
        /// Returns the CRUD (Create, Read, Update, Delete) operation type 
        /// associated with the current request.
        /// </summary>
        public RequestMethod RequestMethod { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="requestMethod">The request method.</param>
        public MethodAttribute(RequestMethod requestMethod)
        {
            RequestMethod = requestMethod;
        }
    }
}
