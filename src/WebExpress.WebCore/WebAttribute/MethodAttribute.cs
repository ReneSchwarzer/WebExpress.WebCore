using System;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies the HTTP method or CRUD operation that an endpoint method
    /// is intended to handle. This attribute can be applied multiple times
    /// to the same method to declare support for multiple request methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MethodAttribute : Attribute, IEndpointAttribute
    {
        /// <summary>
        /// Returns the CRUD (Create, Read, Update, Delete) operation or request
        /// method associated with the decorated endpoint method.
        /// </summary>
        public RequestMethod RequestMethod { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodAttribute"/> class
        /// with the specified request method.
        /// </summary>
        /// <param name="requestMethod">
        /// The request method that the endpoint method should handle.
        /// </param>
        public MethodAttribute(RequestMethod requestMethod)
        {
            RequestMethod = requestMethod;
        }
    }
}
