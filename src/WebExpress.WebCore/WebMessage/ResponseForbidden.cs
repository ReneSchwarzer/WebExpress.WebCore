namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// siehe RFC 2616 Tz. 6
    /// </summary>
    public class ResponseForbidden : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseForbidden()
        {
            var content = "<html><head><title>403</title></head><body>403 - Forbidden</body></html>";
            Status = 403;
            Reason = "Forbidden";

            Header.ContentType = "text/html";
            Header.ContentLength = content.Length;
            Content = content;
        }
    }
}
