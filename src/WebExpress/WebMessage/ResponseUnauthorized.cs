namespace WebExpress.WebMessage
{
    /// <summary>
    /// siehe RFC 2616 Tz. 6
    /// </summary>
    public class ResponseUnauthorized : Response
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseUnauthorized()
        {
            Status = 401;
            Reason = "OK";

            Header.WWWAuthenticate = true;
        }
    }
}
