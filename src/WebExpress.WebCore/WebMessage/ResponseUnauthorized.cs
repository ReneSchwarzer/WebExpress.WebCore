using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebStatusPage;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a response indicating that the request requires user authentication. See RFC 2616 Section 6
    /// </summary>
    [StatusCode(401)]
    public class ResponseUnauthorized : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseUnauthorized()
            : this(null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="message">The user defined status message or null.</param></param>
        public ResponseUnauthorized(StatusMessage message)
        {
            Reason = "OK";

            Header.WWWAuthenticate = true;
        }
    }
}
