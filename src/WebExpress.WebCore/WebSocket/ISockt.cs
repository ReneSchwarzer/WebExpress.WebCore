using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WebExpress.WebCore.WebEndpoint;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Defines the contract for WebSocket endpoints.
    /// </summary>
    public interface ISocket : IEndpoint, IDisposable
    {
        /// <summary>
        /// Invoked after the websocket handshake has been accepted.
        /// Implementers may use the optional cancellation token to abort long-running startup tasks.
        /// the optional connectMessage provides initial metadata from the client (may be null).
        /// </summary>
        /// <param name="connectMessage">Optional initial message or metadata sent by the client during/after connect.</param>
        /// <param name="cancellationToken">A token to cancel startup work.</param>
        /// <returns>An asynchronous task.</returns>
        Task OnConnectedAsync(ISocketMessage connectMessage = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Invoked for each received message (complete message or assembled fragments).
        /// Implementations receive a parsed SocketMessage rather than raw byte buffers.
        /// </summary>
        /// <param name="message">The parsed message originated from the client.</param>
        /// <param name="cancellationToken">Cancellation token for cooperative cancellation.</param>
        /// <returns>An asynchronous task.</returns>
        Task OnReceiveAsync(ISocketMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Invoked when the websocket connection is closed or is about to be closed.
        /// Implementers should perform cleanup and release resources.
        /// </summary>
        /// <param name="closeStatus">Optional close status.</param>
        /// <param name="closeDescription">Optional close description.</param>
        /// <returns>An asynchronous task.</returns>
        Task OnDisconnectedAsync(WebSocketCloseStatus closeStatus, string closeDescription);

        /// <summary>
        /// Invoked when an unhandled exception occurs during websocket processing.
        /// Implementers should use this to log and perform cleanup.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>An asynchronous task.</returns>
        Task OnErrorAsync(Exception exception);
    }
}