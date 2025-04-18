using WebExpress.WebCore.WebAttribute;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a response according to RFC 2616 Section 6.
    /// </summary>
    [StatusCode(301)]
    public class ResponseRedirectPermanentlyMoved : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseRedirectPermanentlyMoved(string location)
        {
            Reason = "permanently moved";

            Header.Location = location;
        }
    }
}
