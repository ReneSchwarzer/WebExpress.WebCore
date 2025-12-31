using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Encapsulates a System.Net.WebSockets.WebSocket instance and supports both text and 
    /// binary messages.
    /// </summary>
    internal class SocketConnection : ISocketConnection, IDisposable
    {
        private readonly System.Net.WebSockets.WebSocket _socket;
        private readonly CancellationTokenSource _cts = new();
        private readonly int _bufferSize;
        private bool _disconnectRaised;

        /// <summary>
        /// Raised when a text message is received.
        /// </summary>
        public event Action<string> TextMessageReceived;

        /// <summary>
        /// Raised when a binary message is received.
        /// </summary>
        public event Action<byte[]> BinaryMessageReceived;

        /// <summary>
        /// Raised when the WebSocket connection is closed or aborted.
        /// </summary>
        public event Action<SocketCloseInfo> Disconnected;

        /// <summary>
        /// Initializes a new instance using a raw network stream and the provided socket context.
        /// </summary>
        /// <param name="networkStream">
        /// The underlying network stream used to create the WebSocket instance.
        /// </param>
        /// <param name="socketContext">
        /// Provides WebSocket configuration such as supported subprotocols.
        /// </param>
        /// <param name="bufferSize">
        /// The size of the internal receive buffer in bytes. The default is 8192.
        /// </param>
        public SocketConnection(Stream networkStream, ISocketContext socketContext, int bufferSize = 8192)
        {
            var options = new WebSocketCreationOptions()
            {
                IsServer = true,
                SubProtocol = socketContext.SupportedSubProtocols.Any()
                    ? string.Join(";", socketContext.SupportedSubProtocols)
                    : null
            };

            _socket = System.Net.WebSockets.WebSocket.CreateFromStream(networkStream, options)
                ?? throw new ArgumentNullException(nameof(networkStream));

            _bufferSize = bufferSize;
        }

        /// <summary>
        /// Sends a text message (UTF-8) over the WebSocket.
        /// </summary>
        /// <param name="message">The text message to send.</param>
        /// <param name="cancellation">A token used to cancel the send operation.</param>
        /// <returns>
        /// A task that represents the asynchronous send operation.
        /// </returns>
        public async Task SendTextAsync(string message, CancellationToken cancellation = default)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await _socket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellation);
        }

        /// <summary>
        /// Sends binary data over the WebSocket.
        /// </summary>
        /// <param name="data">The binary payload to send.</param>
        /// <param name="cancellation">A token used to cancel the send operation.</param>
        /// <returns>
        /// A task that represents the asynchronous send operation.
        /// </returns>
        public async Task SendBinaryAsync(byte[] data, CancellationToken cancellation = default)
        {
            await _socket.SendAsync(data, WebSocketMessageType.Binary, true, cancellation);
        }

        /// <summary>
        /// Internal loop for receiving messages. Invokes the appropriate events for each message.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous receive loop.
        /// </returns>
        internal async Task ReceiveLoopAsync()
        {
            var buffer = new byte[_bufferSize];
            using var builder = new MemoryStream();

            while (_socket.State == WebSocketState.Open && !_cts.IsCancellationRequested)
            {
                WebSocketReceiveResult result;

                try
                {
                    result = await _socket.ReceiveAsync(buffer, _cts.Token);
                }
                catch
                {
                    RaiseDisconnected(WebSocketCloseStatus.InternalServerError, "connection aborted");
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    RaiseDisconnected(result.CloseStatus, result.CloseStatusDescription);
                    break;
                }

                // accumulate fragments
                builder.Write(buffer, 0, result.Count);

                if (result.EndOfMessage)
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(builder.ToArray());
                        TextMessageReceived?.Invoke(message);
                    }
                    else if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        BinaryMessageReceived?.Invoke(builder.ToArray());
                    }

                    builder.SetLength(0);
                }
            }
        }

        /// <summary>
        /// Asynchronously closes the underlying WebSocket connection.
        /// </summary>
        /// <param name="reason">
        /// A textual description sent to the client as part of the close frame.
        /// </param>
        /// <param name="cancellation">
        /// A token used to cancel the close operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous close operation.
        /// </returns>
        public async Task CloseAsync(string reason = "closed", CancellationToken cancellation = default)
        {
            _cts.Cancel();

            if (_socket.State == WebSocketState.Open || _socket.State == WebSocketState.CloseReceived)
            {
                try
                {
                    await _socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, reason, cancellation);
                }
                catch
                {
                    // ignore
                }
            }

            RaiseDisconnected(WebSocketCloseStatus.NormalClosure, reason);
        }

        /// <summary>
        /// Ensures the Disconnected event is raised exactly once.
        /// </summary>
        /// <param name="status">The WebSocket close status.</param>
        /// <param name="reason">The reason for the disconnection.</param>
        private void RaiseDisconnected(WebSocketCloseStatus? status, string reason)
        {
            if (_disconnectRaised)
            {
                return;
            }

            _disconnectRaised = true;
            Disconnected?.Invoke(new SocketCloseInfo(status ?? WebSocketCloseStatus.Empty, reason));
        }

        /// <summary>
        /// Releases all resources used by the socket.
        /// </summary>
        public void Dispose()
        {
            _cts.Cancel();

            try
            {
                if (_socket.State == WebSocketState.Open)
                {
                    _socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "disposed", CancellationToken.None)
                           .Wait(50);
                }
            }
            catch
            {
                // ignore
            }

            RaiseDisconnected(WebSocketCloseStatus.NormalClosure, "disposed");

            if (_socket is IDisposable d)
            {
                d.Dispose();
            }

            _cts.Dispose();
        }
    }
}