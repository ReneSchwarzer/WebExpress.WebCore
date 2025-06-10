using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebStatusPage;
using WebExpress.WebCore.WebUri;

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
        {
        }

        /// <summary>
        /// Initializes a new instance of the class with the specified location.
        /// </summary>
        /// <remarks>The <paramref name="location"/> parameter specifies the new location of the resource.</remarks>
        /// <param name="location">The URI to which the resource has been moved permanently. This value cannot be <see langword="null"/>.</param>
        public ResponseMovedPermanently(IUri location)
        {
            Reason = "moved permanently";
            Header.Location = location?.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="message">The user defined status message or null.</param>
        public ResponseMovedPermanently(StatusMessage message)
        {
            var content = message?.Message ?? "<html><head><title>404</title></head><body>301 - Moved Permanently</body></html>";
            Reason = "moved permanently";

            Header.ContentType = "text/html";
            Header.ContentLength = content.Length;
            Content = content;
        }
    }
}
