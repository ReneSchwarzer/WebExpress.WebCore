using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Represents an asynchronous read-only stream abstraction for receiving
    /// WebSocket message data in one or more frames using the native
    /// WebExpress WebSocket protocol.
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
        /// A <see cref="SocketReceiveResult"/> describing the number of bytes read,
        /// whether the message has ended, and the message type.
        /// </returns>
        Task<SocketReceiveResult> ReadAsync
        (
            ArraySegment<byte> buffer,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Signals that the current message has been fully consumed.
        /// After calling this method, no further reads for the current
        /// message are allowed.
        /// </summary>
        Task CompleteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously closes the underlying WebSocket connection.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="description">An optional description for the closure.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous close operation.</returns>
        Task CloseAsync
        (
            SocketCloseStatus status = SocketCloseStatus.NormalClosure,
            string description = null,
            CancellationToken cancellationToken = default
        );
    }
}