namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Represents information about the reason a socket connection was closed, 
    /// including the close status and an optional description.
    /// </summary>
    public class SocketCloseInfo
    {
        /// <summary>
        /// Returns the status that indicates the reason the socket was closed.
        /// </summary>
        public SocketCloseStatus Status { get; }

        /// <summary>
        /// Returns the description associated with this instance.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the SocketCloseInfo class with the specified 
        /// close status and optional description.
        /// </summary>
        /// <param name="status">
        /// The status code indicating the reason the socket was closed.
        /// </param>
        /// <param name="description">
        /// An optional textual description providing additional details about the 
        /// socket closure. May be null.
        /// </param>
        public SocketCloseInfo(SocketCloseStatus status, string description)
        {
            Status = status;
            Description = description;
        }
    }
}