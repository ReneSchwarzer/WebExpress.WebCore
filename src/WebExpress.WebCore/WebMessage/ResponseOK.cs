namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// siehe RFC 2616 Tz. 6
    /// </summary>
    public class ResponseOK : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseOK()
        {
            Status = 200;
            Reason = "OK";
        }
    }
}
