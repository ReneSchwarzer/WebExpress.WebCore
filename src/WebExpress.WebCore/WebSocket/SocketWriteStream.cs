using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Provides a WebSocket-based implementation of <see cref="ISocketWriteStream"/>.
    /// Allows writing message data in one or more frames before finalizing the message.
    /// </summary>
    public class SocketWriteStream : ISocketWriteStream
    {
        private readonly System.Net.WebSockets.WebSocket _socket;
        private readonly WebSocketMessageType _messageType;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="socket">The underlying WebSocket transport.</param>
        /// <param name="messageType">The WebSocket message type to use for all frames.</param>
        public SocketWriteStream(System.Net.WebSockets.WebSocket socket, WebSocketMessageType messageType)
        {
            _socket = socket;
            _messageType = messageType;
        }

        /// <summary>
        /// Writes a chunk of data to the underlying WebSocket transport.
        /// This method does not finalize the message; callers may invoke it
        /// multiple times to send a message in fragments.
        /// </summary>
        /// <param name="buffer">The data buffer to write.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        public async Task WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (_socket.State != WebSocketState.Open)
            {
                return;
            }

            await _socket.SendAsync
            (
                buffer,
                _messageType,
                endOfMessage: false,
                cancellationToken
            );
        }

        /// <summary>
        /// Completes the message by sending the final WebSocket frame with
        /// <c>endOfMessage</c> set to <c>true</c>.
        /// After calling this method, no further writes are allowed.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        public async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            if (_socket.State != WebSocketState.Open)
            {
                return;
            }

            // Send an empty frame marking the end of the message.
            await _socket.SendAsync
            (
                ReadOnlyMemory<byte>.Empty,
                _messageType,
                endOfMessage: true,
                cancellationToken
            );
        }

        /// <summary> 
        /// Performs application-defined tasks associated with freeing, releasing, 
        /// or resetting unmanaged resources asynchronously. 
        /// </summary> 
        /// <returns> 
        /// A completed <see cref="ValueTask"/> because this implementation does not 
        /// hold unmanaged resources. 
        /// </returns>
        public ValueTask DisposeAsync()
        {
            // No unmanaged resources to release.
            return ValueTask.CompletedTask;
        }
    }
}