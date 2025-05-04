using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a response according to RFC 2616 Section 6.
    /// </summary>
    [StatusCode(302)]
    public class ResponseRedirectTemporarilyMoved : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseRedirectTemporarilyMoved(IUri location)
        {
            Reason = "temporarily moved";
            //Content = "<html></html>";

            //HeaderFields.ContentType = "text/html";
            //HeaderFields.ContentLength = Content.ToString().Length;
            Header.Location = location?.ToString();
        }
    }
}
