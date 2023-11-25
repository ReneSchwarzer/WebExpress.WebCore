namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// siehe RFC 2616 Tz. 6
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Setzt oder liefert die Optionen
        /// </summary>
        public ResponseHeaderFields Header { get; } = new ResponseHeaderFields();

        /// <summary>
        /// Setzt oder liefert den Content
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Liefert oder setzt den Statuscode
        /// </summary>
        public int Status { get; protected set; }

        /// <summary>
        /// Liefert oder setzt den Statustext
        /// </summary>
        public string Reason { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected Response()
        {
        }
    }
}
