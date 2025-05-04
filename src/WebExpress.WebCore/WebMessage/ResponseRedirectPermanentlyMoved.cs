using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebUri;

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
        public ResponseRedirectPermanentlyMoved(IUri location)
        {
            Reason = "permanently moved";

            Header.Location = location?.ToString();
        }
    }
}
