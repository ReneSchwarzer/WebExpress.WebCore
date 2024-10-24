using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebStatusPage;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a response for a resource moved permanently (301) according to RFC 2616 Section 6.
    /// </summary>
    [StatusCode(301)]
    public class ResponseMovedPermanently : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseMovedPermanently()
            : this(null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="message">The user defined status message or null.</param></param>
        public ResponseMovedPermanently(StatusMessage message)
        {
            var content = message?.Message ?? "<html><head><title>404</title></head><body>301 - Moved Permanently</body></html>";
            Reason = "Moved Permanently";

            Header.ContentType = "text/html";
            Header.ContentLength = content.Length;
            Content = content;
        }
    }
}
