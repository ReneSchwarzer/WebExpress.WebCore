namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Represents a parsed WebSocket frame in the native
    /// WebExpress WebSocket protocol implementation.
    /// </summary>
    public class SocketFrame
    {
        /// <summary>
        /// Indicates whether this frame is the final frame of the message.
        /// </summary>
        public bool Fin { get; set; }

        /// <summary>
        /// The message type of the frame (text, binary, close, ping, pong, continuation).
        /// </summary>
        public SocketMessageType MessageType { get; set; }

        /// <summary>
        /// Indicates whether the payload is masked.
        /// Client-to-server frames must be masked; server-to-client frames are not.
        /// </summary>
        public bool Masked { get; set; }

        /// <summary>
        /// The raw payload data of the frame.
        /// </summary>
        public byte[] Payload { get; set; } = [];
    }
}