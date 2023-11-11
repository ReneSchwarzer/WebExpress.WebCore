namespace WebExpress.Core.WebMessage
{
    /// <summary>
    /// siehe RFC 2616 Tz. 6
    /// </summary>
    public class ResponseRedirectPermanentlyMoved : Response
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseRedirectPermanentlyMoved(string location)
        {
            Status = 301;
            Reason = "permanently moved";

            Header.Location = location;
        }
    }
}
