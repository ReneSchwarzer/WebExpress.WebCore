namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// siehe RFC 2616 Tz. 6
    /// </summary>
    public class ResponseUnauthorized : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseUnauthorized()
        {
            Status = 401;
            Reason = "OK";

            Header.WWWAuthenticate = true;
        }
    }
}
