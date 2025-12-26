using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Represents a WebSocket connection with send/receive logic
    /// using the native WebExpress WebSocket protocol.
    /// </summary>
    public class Socket
    {
        private readonly Stream _stream;
        private readonly CancellationToken _token;

        /// <summary>
        /// Provides access to the underlying data stream.
        /// </summary>
        internal Stream Stream => _stream;

        /// <summary>
        /// Occurs when a text message is received, allowing subscribers to handle the message asynchronously.
        /// </summary>
        public event Func<string, Task> OnTextMessage;

        /// <summary>
        /// Occurs when a binary message is received, providing the message data as a byte array.
        /// </summary>
        public event Func<byte[], Task> OnBinaryMessage;

        /// <summary>
        /// Occurs when the component is being closed, allowing subscribers to perform asynchronous cleanup or
        /// finalization tasks.
        /// </summary>
        public event Func<Task> OnClose;

        /// <summary>
        /// Initializes a new instance of the Socket class using the specified data 
        /// stream and cancellation token.
        /// </summary>
        /// <param name="stream">
        /// The stream to use for network communication. Must be readable and writable.
        /// </param>
        /// <param name="token">
        /// A cancellation token that can be used to cancel operations associated 
        /// with this socket.
        /// </param>
        public Socket(Stream stream, CancellationToken token)
        {
            _stream = stream;
            _token = token;
        }

        /// <summary>
        /// Starts reading frames from the underlying stream and dispatches
        /// them to the appropriate event handlers.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task StartAsync()
        {
            while (!_token.IsCancellationRequested)
            {
                var frame = SocketFrameParser.ReadFrame(_stream);

                switch (frame.MessageType)
                {
                    case SocketMessageType.Text:
                        var text = Encoding.UTF8.GetString(frame.Payload);
                        if (OnTextMessage != null)
                        {
                            await OnTextMessage(text);
                        }
                        break;

                    case SocketMessageType.Binary:
                        if (OnBinaryMessage != null)
                        {
                            await OnBinaryMessage(frame.Payload);
                        }
                        break;

                    case SocketMessageType.Close:
                        if (frame is SocketFrameClose close)
                        {
                            await SendCloseAsync(close.Status, close.Description);
                        }
                        else
                        {
                            await SendCloseAsync(SocketCloseStatus.NormalClosure, "closing");
                        }

                        if (OnClose != null)
                        {
                            await OnClose();
                        }

                        return;

                    case SocketMessageType.Ping:
                        await SendPongAsync(frame.Payload);
                        break;

                    case SocketMessageType.Pong:
                        break;

                    case SocketMessageType.Continuation:
                        // optional: handle fragmented messages
                        break;
                }
            }
        }

        /// <summary>
        /// Asynchronously sends a text message over the WebSocket connection.
        /// </summary>
        /// <param name="message">The text message to send. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous send operation.</returns>
        public Task SendTextAsync(string message)
        {
            var payload = Encoding.UTF8.GetBytes(message);
            return SendFrameAsync(SocketMessageType.Text, payload);
        }

        /// <summary>
        /// Asynchronously sends a binary message to the connected endpoint.
        /// </summary>
        /// <param name="data">The binary data to send. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous send operation.</returns>
        public Task SendBinaryAsync(byte[] data)
        {
            return SendFrameAsync(SocketMessageType.Binary, data);
        }

        /// <summary>
        /// Initiates an asynchronous close handshake by sending a WebSocket close frame 
        /// to the remote endpoint.
        /// </summary>
        /// <param name="status">
        /// The status code indicating the reason for closure.
        /// </param>
        /// <param name="description">
        /// An optional description providing additional context for the closure.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous close operation.
        /// </returns>
        public Task SendCloseAsync(SocketCloseStatus status, string description = null)
        {
            byte[] reasonBytes = description != null
                ? Encoding.UTF8.GetBytes(description)
                : [];

            byte[] payload = new byte[2 + reasonBytes.Length];

            // statuscode (2 byte, big endian)
            payload[0] = (byte)((ushort)status >> 8);
            payload[1] = (byte)((ushort)status & 0xFF);

            if (reasonBytes.Length > 0)
            {
                Array.Copy(reasonBytes, 0, payload, 2, reasonBytes.Length);
            }

            return SendFrameAsync(SocketMessageType.Close, payload);
        }

        /// <summary>
        /// Sends a WebSocket Pong frame asynchronously with the specified payload.
        /// </summary>
        /// <param name="payload">
        /// The optional application data to include in the Pong frame. May be null 
        /// or empty if no payload is required.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous send operation.
        /// </returns>
        public Task SendPongAsync(byte[] payload)
        {
            return SendFrameAsync(SocketMessageType.Pong, payload);
        }

        /// <summary>
        /// Asynchronously sends a WebSocket frame with the specified message type 
        /// and payload over the underlying stream.
        /// </summary>
        /// <param name="type">
        /// The type of the WebSocket message to send. Determines the opcode set in 
        /// the frame header.
        /// </param>
        /// <param name="payload">
        /// The payload data to include in the frame. Must not be null.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous send operation.
        /// </returns>
        private async Task SendFrameAsync(SocketMessageType type, byte[] payload)
        {
            using var ms = new MemoryStream();

            // FIN + opcode
            ms.WriteByte((byte)(0b1000_0000 | type.ToOpcode()));

            // payload length
            if (payload.Length < 126)
            {
                ms.WriteByte((byte)payload.Length);
            }
            else if (payload.Length <= ushort.MaxValue)
            {
                ms.WriteByte(126);
                var len = BitConverter.GetBytes((ushort)payload.Length);
                if (BitConverter.IsLittleEndian) { Array.Reverse(len); }
                ms.Write(len);
            }
            else
            {
                ms.WriteByte(127);
                var len = BitConverter.GetBytes((ulong)payload.Length);
                if (BitConverter.IsLittleEndian) { Array.Reverse(len); }
                ms.Write(len);
            }

            // payload
            ms.Write(payload);

            var buffer = ms.ToArray();
            await _stream.WriteAsync(buffer, 0, buffer.Length, _token);
            await _stream.FlushAsync(_token);
        }
    }
}