using System;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Represents an error that occurs when a WebSocket handshake request
    /// is invalid or does not meet the required protocol specifications.
    /// </summary>
    public class SocketHandshakeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SocketHandshakeException"/> class
        /// with the specified error message.
        /// </summary>
        /// <param name="message">
        /// A descriptive message that explains the reason for the handshake failure.
        /// </param>
        public SocketHandshakeException(string message)
            : base(message)
        {
        }
    }
}
