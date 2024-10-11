using WebExpress.WebCore.WebAttribute;

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
        {
            Reason = "OK";

            Header.WWWAuthenticate = true;
        }
    }
}
