using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// A binary-oriented implementation of <see cref="ISocketReadStream"/>,
    /// exposing incoming WebSocket message data as raw byte segments using
    /// the native WebExpress WebSocket protocol.
    /// </summary>
    public sealed class SocketReadStream : ISocketReadStream
    {
        private readonly Socket _socket;
        private readonly ISocketContext _socketContext;
        private readonly string _connectionId;

        private SocketFrame _currentFrame;
        private int _frameOffset = 0;

        /// <summary>
        /// Returns the context associated with the underlying socket connection.
        /// </summary>
        public ISocketContext SocketContext => _socketContext;

        /// <summary>
        /// Returns the unique identifier for the current connection.
        /// </summary>
        public string ConnectionId => _connectionId;

        /// <summary>
        /// Initializes a new instance of the SocketReadStream class for reading data
        /// from a native WebExpress WebSocket connection.
        /// </summary>
        public SocketReadStream(Socket socket, ISocketContext socketContext, string connectionId)
        {
            _socket = socket ?? throw new ArgumentNullException(nameof(socket));
            _socketContext = socketContext;
            _connectionId = connectionId;
        }

        /// <summary>
        /// Reads a chunk of data from the underlying WebSocket transport.
        /// Supports fragmented messages by returning partial payload segments.
        /// </summary>
        public async Task<SocketReceiveResult> ReadAsync(
            ArraySegment<byte> buffer,
            CancellationToken cancellationToken = default)
        {
            // Load a new frame if needed
            if (_currentFrame == null)
            {
                _currentFrame = await _socket.ReadFrameAsync();
                _frameOffset = 0;
            }

            var payload = _currentFrame.Payload;

            // Remaining bytes in this frame
            int remaining = payload.Length - _frameOffset;

            if (remaining <= 0)
            {
                // End of message
                var messageType = _currentFrame.MessageType;
                _currentFrame = null;

                return new SocketReceiveResult(
                    count: 0,
                    endOfMessage: true,
                    messageType: messageType
                );
            }

            // Copy as much as fits into the buffer
            int toCopy = Math.Min(buffer.Count, remaining);

            Array.Copy(
                payload,
                _frameOffset,
                buffer.Array!,
                buffer.Offset,
                toCopy
            );

            _frameOffset += toCopy;

            bool endOfMessage = _frameOffset >= payload.Length;
            var type = _currentFrame.MessageType;

            if (endOfMessage)
            {
                _currentFrame = null;
            }

            return new SocketReceiveResult(
                count: toCopy,
                endOfMessage: endOfMessage,
                messageType: type
            );
        }

        /// <summary>
        /// Marks the current message as fully consumed.
        /// For the native protocol, this is a no-op.
        /// </summary>
        public Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Asynchronously closes the underlying WebSocket connection.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="description">An optional description for the closure.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous close operation.</returns>
        public Task CloseAsync
        (
            SocketCloseStatus status = SocketCloseStatus.NormalClosure,
            string description = null,
            CancellationToken cancellationToken = default
        )
        {
            return _socket.SendCloseAsync(status, description);
        }

        /// <summary>
        /// Performs cleanup operations for the read stream.
        /// </summary>
        public ValueTask DisposeAsync()
        {
            try
            {
                _socket.SendCloseAsync(SocketCloseStatus.NormalClosure, "disposing");
            }
            catch
            {
                // Socket already closed or broken – ignore
            }

            return ValueTask.CompletedTask;
        }
    }
}