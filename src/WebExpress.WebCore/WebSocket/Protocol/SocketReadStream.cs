using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// A binary-oriented implementation of <see cref="ISocketReadStream"/>,
    /// exposing incoming WebSocket message data as raw byte segments using
    /// the RFC 6455 WebSocket protocol.
    /// </summary>
    public sealed class SocketReadStream : ISocketReadStream
    {
        private readonly System.Net.WebSockets.WebSocket _webSocket;
        private readonly ISocketContext _socketContext;
        private readonly string _connectionId;

        private WebSocketReceiveResult _lastResult = null;

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
        /// from a WebSocket connection.
        /// </summary>
        /// <param name="webSocket">The WebSocket instance.</param>
        /// <param name="socketContext">The logical socket context.</param>
        /// <param name="connectionId">The connection identifier.</param>
        public SocketReadStream(System.Net.WebSockets.WebSocket webSocket, ISocketContext socketContext, string connectionId)
        {
            _webSocket = webSocket ?? throw new ArgumentNullException(nameof(webSocket));
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
            // reads data from the WebSocket instance into the provided buffer
            _lastResult = await _webSocket.ReceiveAsync(buffer, cancellationToken);

            return new SocketReceiveResult(
                count: _lastResult.Count,
                endOfMessage: _lastResult.EndOfMessage,
                messageType: _lastResult.MessageType switch
                {
                    WebSocketMessageType.Binary => SocketMessageType.Binary,
                    WebSocketMessageType.Text => SocketMessageType.Text,
                    WebSocketMessageType.Close => SocketMessageType.Close,
                    _ => SocketMessageType.Binary
                }
            );
        }

        /// <summary>
        /// Marks the current message as fully consumed.
        /// For the standard WebSocket protocol, this is a no-op.
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
        public async Task CloseAsync
        (
            WebSocketCloseStatus status = WebSocketCloseStatus.NormalClosure,
            string description = null,
            CancellationToken cancellationToken = default
        )
        {
            await _webSocket.CloseAsync(
                closeStatus: status,
                statusDescription: description,
                cancellationToken: cancellationToken
            );
        }

        /// <summary>
        /// Performs cleanup operations for the read stream.
        /// </summary>
        /// <returns>A value task indicating the stream was disposed.</returns>
        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_webSocket != null && _webSocket.State != WebSocketState.Closed && _webSocket.State != WebSocketState.Aborted)
                {
                    await _webSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "disposing",
                        CancellationToken.None
                    );
                }
            }
            catch
            {
                // socket already closed or broken – ignore
            }
        }
    }
}