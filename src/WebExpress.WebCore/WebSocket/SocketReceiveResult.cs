namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Represents the result of a read operation on a WebSocket stream,
    /// containing the number of bytes read, the message type, and whether
    /// the end of the message has been reached.
    /// </summary>
    public class SocketReceiveResult
    {
        /// <summary>
        /// The number of bytes read into the provided buffer.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Indicates whether the end of the current WebSocket message has been reached.
        /// </summary>
        public bool EndOfMessage { get; }

        /// <summary>
        /// The type of the WebSocket message (text, binary, close, ping, pong, continuation).
        /// </summary>
        public SocketMessageType MessageType { get; }

        /// <summary>
        /// Initializes a new instance of the class with the specified number of 
        /// bytes received, end-of-message indicator, and message type.
        /// </summary>
        /// <param name="count">
        /// The number of bytes received in the operation.
        /// </param>
        /// <param name="endOfMessage">
        /// True if the received data marks the end of the message; otherwise, false.
        /// </param>
        /// <param name="messageType">
        /// The type of message received, indicating how the data should be interpreted.
        /// </param>
        public SocketReceiveResult(int count, bool endOfMessage, SocketMessageType messageType)
        {
            Count = count;
            EndOfMessage = endOfMessage;
            MessageType = messageType;
        }
    }
}