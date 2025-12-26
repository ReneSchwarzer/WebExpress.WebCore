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
        /// <param name="socket">The WebExpress WebSocket wrapper.</param>
        /// <param name="socketContext">The logical socket context.</param>
        /// <param name="connectionId">The connection identifier.</param>
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
        /// <param name="buffer">The buffer receiving the data.</param>
        /// <param name="cancellationToken">The cancellation token for the async read operation.</param>
        /// <returns>A result indicating the bytes read and message boundaries.</returns>
        public async Task<SocketReceiveResult> ReadAsync
        (
            ArraySegment<byte> buffer,
            CancellationToken cancellationToken = default
        )
        {
            // load a new frame if needed
            if (_currentFrame == null)
            {
                // uses the internal stream from the Socket class
                _currentFrame = await Task.Run(() => SocketFrameParser.ReadFrame(_socket.Stream), cancellationToken);
                _frameOffset = 0;
            }

            var payload = _currentFrame.Payload;

            // remaining bytes in this frame
            int remaining = payload.Length - _frameOffset;

            if (remaining <= 0)
            {
                // end of message
                var messageType = _currentFrame.MessageType;
                _currentFrame = null;

                return new SocketReceiveResult(
                    count: 0,
                    endOfMessage: true,
                    messageType: messageType
                );
            }

            // copy as much as fits into the buffer
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
        /// <param name="cancellationToken">The cancellation token (unused).</param>
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
        /// <returns>A value task indicating the stream was disposed.</returns>
        public ValueTask DisposeAsync()
        {
            try
            {
                _socket.SendCloseAsync(SocketCloseStatus.NormalClosure, "disposing");
            }
            catch
            {
                // socket already closed or broken – ignore
            }

            return ValueTask.CompletedTask;
        }
    }
}