namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Defines the type of WebSocket message represented or sent
    /// in the native WebExpress WebSocket protocol.
    /// </summary>
    public enum SocketMessageType
    {
        /// <summary>
        /// Specifies that the data is UTF‑8 encoded text.
        /// </summary>
        Text,

        /// <summary>
        /// Specifies that the data is binary.
        /// </summary>
        Binary
    }
}