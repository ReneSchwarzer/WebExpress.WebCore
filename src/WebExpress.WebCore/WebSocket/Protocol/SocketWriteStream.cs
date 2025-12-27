using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Provides a write stream abstraction for sending WebSocket messages
    /// using the native WebExpress WebSocket protocol implementation.
    /// </summary>
    public class SocketWriteStream : ISocketWriteStream
    {
        private readonly System.Net.WebSockets.WebSocket _socket;
        private readonly SocketMessageType _messageType;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="socket">The underlying native web socket connection.</param>
        /// <param name="messageType">The message type (text or binary).</param>
        public SocketWriteStream(System.Net.WebSockets.WebSocket socket, SocketMessageType messageType)
        {
            _socket = socket;
            _messageType = messageType;
        }

        /// <summary>
        /// Writes a chunk of data to the underlying web socket transport.
        /// This method does not finalize the message; callers may invoke it
        /// multiple times to send a message in fragments.
        /// </summary>
        /// <param name="buffer">The data buffer to write.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        public async Task WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            //if (_messageType == SocketMessageType.Text)
            //{
            //    // Convert bytes to UTF8 text
            //    var text = System.Text.Encoding.UTF8.GetString(buffer.Span);
            //    await _socket.SendTextAsync(text);
            //}
            //else
            //{
            //    await _socket.SendBinaryAsync(buffer.ToArray());
            //}
        }

        /// <summary>
        /// Completes the message. For the native protocol implementation,
        /// messages are finalized automatically, so this method performs no action.
        /// </summary>
        public Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            // No explicit final frame required in the native protocol.
            return Task.CompletedTask;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources asynchronously.
        /// </summary>
        public ValueTask DisposeAsync()
        {
            // No unmanaged resources to release.
            return ValueTask.CompletedTask;
        }
    }
}