using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Represents an asynchronous write-only stream abstraction for sending
    /// WebSocket message data in one or more frames.
    /// </summary>
    public interface ISocketWriteStream : IAsyncDisposable
    {
        /// <summary>
        /// Writes a chunk of data to the underlying WebSocket transport.
        /// This method does not finalize the message; callers may invoke it
        /// multiple times to send a message in fragments.
        /// </summary>
        /// <param name="buffer">The data buffer to write.</param>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        Task WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default);

        /// <summary>
        /// Completes the message by sending the final WebSocket frame with
        /// <c>endOfMessage</c> set to <c>true</c>.
        /// After calling this method, no further writes are allowed.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        Task CompleteAsync(CancellationToken cancellationToken = default);
    }
}