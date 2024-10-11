using WebExpress.WebCore.WebAttribute;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a response for the HTTP 500 Internal Server Error status code according to RFC 2616 Section 6.
    /// </summary>
    [StatusCode(500)]
    public class ResponseInternalServerError : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseInternalServerError()
        {
            var content = "<html><head><title>404</title></head><body>500 - Internal Server Error</body></html>";
            Reason = "Internal Server Error";

            Header.ContentType = "text/html";
            Header.ContentLength = content.Length;
            Content = content;
        }
    }
}
