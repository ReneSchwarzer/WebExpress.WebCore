using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Defines the contract for a socket connection that supports sending and receiving both text and binary messages.
    /// </summary>
    public interface ISocketConnection : IDisposable
    {
        /// <summary>
        /// Raised when a text message is received.
        /// </summary>
        event Action<string> TextMessageReceived;

        /// <summary>
        /// Raised when a binary message is received.
        /// </summary>
        event Action<byte[]> BinaryMessageReceived;

        /// <summary>
        /// Raised when the WebSocket connection is closed or aborted.
        /// </summary>
        event Action<SocketCloseInfo> Disconnected;

        /// <summary>
        /// Sends a text message (UTF-8) over the socket connection.
        /// </summary>
        /// <param name="message">The text message to send.</param>
        /// <param name="cancellation">Optional cancellation token.</param>
        Task SendTextAsync(string message, CancellationToken cancellation = default);

        /// <summary>
        /// Sends binary data over the socket connection.
        /// </summary>
        /// <param name="data">The binary data to send.</param>
        /// <param name="cancellation">Optional cancellation token.</param>
        Task SendBinaryAsync(byte[] data, CancellationToken cancellation = default);

        /// <summary>
        /// Closes the socket connection gracefully.
        /// </summary>
        /// <param name="reason">The reason for closing the connection.</param>
        /// <param name="cancellation">Optional cancellation token.</param>
        Task CloseAsync(string reason = "closed", CancellationToken cancellation = default);
    }
}
