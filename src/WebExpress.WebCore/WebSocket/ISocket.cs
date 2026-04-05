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
        /// </summary>
        /// <param name="socketConnection">The socket connection.</param>
        /// <returns>An asynchronous task.</returns>
        Task OnConnectedAsync(ISocketConnection socketConnection);
    }
}