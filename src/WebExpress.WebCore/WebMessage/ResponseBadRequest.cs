namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// siehe RFC 2616 Tz. 6
    /// </summary>
    public class ResponseBadRequest : Response
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseBadRequest()
        {
            var content = "<html><head><title>404</title></head><body>404 - Bad Request</body></html>";
            Status = 400;
            Reason = "Bad Request";

            Header.ContentType = "text/html";
            Header.ContentLength = content.Length;
            Content = content;
        }
    }
}
