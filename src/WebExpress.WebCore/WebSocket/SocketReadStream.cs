using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// A binary-oriented implementation of <see cref="ISocketReadStream"/>,
    /// exposing incoming WebSocket message data as raw byte segments.
    /// </summary>
    public sealed class SocketReadStream : ISocketReadStream
    {
        private readonly System.Net.WebSockets.WebSocket _socket;
        private readonly ISocketContext _socketContext;
        private readonly string _connectionId;

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
        /// <param name="socket">
        /// The WebSocket instance representing the underlying connection. Cannot be null.
        /// </param>
        /// <param name="socketContext">
        /// The context object that provides additional information or services related 
        /// to the socket connection.
        /// </param>
        /// <param name="connectionId">
        /// A unique identifier for the connection associated with this stream.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if the socket parameter is null.</exception>
        public SocketReadStream(System.Net.WebSockets.WebSocket socket, ISocketContext socketContext, string connectionId)
        {
            _socket = socket ?? throw new ArgumentNullException(nameof(socket));
            _socketContext = socketContext;
            _connectionId = connectionId;
        }

        /// <summary>
        /// Receives data asynchronously from the underlying WebSocket and writes it into 
        /// the provided buffer.
        /// </summary>
        /// <param name="buffer">
        /// The buffer that receives the incoming data. The method writes the received 
        /// bytes into this memory region.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used to cancel the receive operation.
        /// </param>
        /// <returns>
        /// A WebSocketReceiveResult that contains information about the received data, 
        /// including the number of bytes read, the message type, and whether the message 
        /// is complete.
        /// </returns>
        public async Task<WebSocketReceiveResult> ReadAsync
        (
            ArraySegment<byte> buffer,
            CancellationToken cancellationToken = default
        )
        {
            var result = await _socket.ReceiveAsync(buffer, cancellationToken)
                .ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Marks the current message as fully consumed.
        /// </summary>
        public Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            // no explicit "complete" frame for reading; this is a no-op.
            return Task.CompletedTask;
        }

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
        public Task CloseAsync(CancellationToken cancellationToken = default)
        {
            return _socket.CloseAsync
            (
                WebSocketCloseStatus.NormalClosure,
                "Read stream closed",
                CancellationToken.None
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
            _socket.CloseAsync
            (
                WebSocketCloseStatus.NormalClosure,
                "Binary read stream closed",
                CancellationToken.None
            );

            return ValueTask.CompletedTask;
        }
    }
}