using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebStatusPage;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a response for a bad request (HTTP 400). See RFC 2616 Section 6
    /// </summary>
    [StatusCode(400)]
    public class ResponseBadRequest : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseBadRequest()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="message">The user defined status message or null.</param></param>
        public ResponseBadRequest(StatusMessage message)
        {
            var content = message?.Message ?? "<html><head><title>404</title></head><body>404 - Bad Request</body></html>";
            Reason = "Bad Request";

            Header.ContentType = "text/html";
            Header.ContentLength = content.Length;
            Content = content;
        }
    }
}
