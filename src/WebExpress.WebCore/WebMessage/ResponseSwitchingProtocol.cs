using WebExpress.WebCore.WebAttribute;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a response for a protocol switch (101) according to RFC 2616 Section 6.
    /// </summary>
    [StatusCode(101)]
    public class ResponseSwitchingProtocols : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="connection">The Connection header value.</param>
        /// <param name="upgrade">The Upgrade header value.</param>
        /// <param name="_secWebSocketAccept">The Sec-WebSocket-Accept header value.</param>
        public ResponseSwitchingProtocols(string connection, string upgrade, string _secWebSocketAccept)
        {
            Reason = "Switching Protocols";
            Header.Upgrade = upgrade;
            Header.Connection = connection;
            Header.SecWebSocketAccept = _secWebSocketAccept;
        }
    }
}