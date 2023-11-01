namespace WebExpress.WebMessage
{
    /// <summary>
    /// siehe RFC 2616 Tz. 6
    /// </summary>
    public class ResponseNotFound : Response
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseNotFound()
        {
            var content = "<html><head><title>404</title></head><body>404 - Not Found</body></html>";
            Status = 404;
            Reason = "Not Found";

            Header.ContentType = "text/html";
            Header.ContentLength = content.Length;
            Content = content;
        }
    }
}
