using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Represents an asynchronous read-only stream abstraction for receiving
    /// WebSocket message data in one or more frames.
    /// </summary>
    public interface ISocketReadStream : IAsyncDisposable
    {
        /// <summary>
        /// Returns the context associated with the underlying socket connection.
        /// </summary>
        ISocketContext SocketContext { get; }

        /// <summary>
        /// Returns the unique identifier for the current connection.
        /// </summary>
        string ConnectionId { get; }

        /// <summary>
        /// Reads a chunk of data from the underlying WebSocket transport.
        /// This method does not assume the message is complete; callers may
        /// invoke it multiple times to receive a fragmented message.
        /// </summary>
        /// <param name="buffer">
        /// The buffer into which the received data will be written.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        /// <returns>
        /// The number of bytes read. Returns 0 if the end of the message
        /// has been reached.
        /// </returns>
        Task<WebSocketReceiveResult> ReadAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken = default);

        /// <summary>
        /// Signals that the current message has been fully consumed.
        /// After calling this method, no further reads for the current
        /// message are allowed.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        Task CompleteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously closes the underlying WebSocket connection using a normal 
        /// closure status.
        /// </summary>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used to cancel the close operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous close operation.
        /// </returns>
        Task CloseAsync(CancellationToken cancellationToken = default);
    }
}