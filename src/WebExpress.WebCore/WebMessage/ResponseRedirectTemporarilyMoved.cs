namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// siehe RFC 2616 Tz. 6
    /// </summary>
    public class ResponseRedirectTemporarilyMoved : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseRedirectTemporarilyMoved(string location)
        {
            Status = 302;
            Reason = "temporarily moved";
            //Content = "<html></html>";

            //HeaderFields.ContentType = "text/html";
            //HeaderFields.ContentLength = Content.ToString().Length;
            Header.Location = location;
        }
    }
}
