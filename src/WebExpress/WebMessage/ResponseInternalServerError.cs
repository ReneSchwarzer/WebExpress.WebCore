namespace WebExpress.WebMessage
{
    /// <summary>
    /// siehe RFC 2616 Tz. 6
    /// </summary>
    public class ResponseInternalServerError : Response
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseInternalServerError()
        {
            var content = "<html><head><title>404</title></head><body>500 - Internal Server Error</body></html>";
            Status = 500;
            Reason = "Internal Server Error";

            Header.ContentType = "text/html";
            Header.ContentLength = content.Length;
            Content = content;
        }
    }
}
