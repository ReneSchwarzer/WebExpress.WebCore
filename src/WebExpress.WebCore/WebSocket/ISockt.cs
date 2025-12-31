using System;
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
        /// <param name="socketConnection">The socket connection.</param>
        /// <returns>An asynchronous task.</returns>
        Task OnConnectedAsync(ISocketConnection socketConnection);
    }
}