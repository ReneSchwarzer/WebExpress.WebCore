using System.Reflection;
using WebExpress.WebCore.WebAttribute;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a response according to RFC 2616 Section 6.
    /// </summary>
    public abstract class Response
    {
        /// <summary>
        /// Returns the response header fields.
        /// </summary>
        public ResponseHeaderFields Header { get; } = new ResponseHeaderFields();

        /// <summary>
        /// Returns or sets the response content.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Returns the status code of the response.
        /// </summary>
        public int Status => GetType().GetCustomAttribute<StatusCodeAttribute>().StatusCode;

        /// <summary>
        /// Returns or sets the reason phrase of the response.
        /// </summary>
        public string Reason { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the Response class.
        /// </summary>
        protected Response()
        {
        }
    }
}
