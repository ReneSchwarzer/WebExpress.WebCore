using WebExpress.WebCore.WebAttribute;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a successful response according to RFC 2616 Section 6.
    /// </summary>
    [StatusCode(200)]
    public class ResponseOK : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseOK()
        {
            Reason = "OK";
        }
    }
}
