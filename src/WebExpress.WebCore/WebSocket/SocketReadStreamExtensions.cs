using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Provides extension methods for reading <see cref="ISocketMessage"/> instances
    /// from an <see cref="ISocketReadStream"/>.
    /// </summary>
    public static class SocketReadStreamExtensions
    {
        /// <summary>
        /// Reads a complete WebSocket message from the stream and deserializes it
        /// into a <see cref="ISocketMessage"/> instance.
        /// </summary>
        /// <param name="stream">The source read stream.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>The deserialized message.</returns>
        public static async Task<ISocketMessage> ReadMessageAsync
        (
            this ISocketReadStream stream,
            CancellationToken cancellationToken = default
        )
        {
            // buffer for accumulating message fragments
            var totalBytes = 0ul;
            using var buffer = new MemoryStream();
            var temp = new byte[4096];
            var messageType = WebSocketMessageType.Text;
            var maxSize = stream.SocketContext?.MaxMessageSize ?? ulong.MinValue;
            var closeStatus = WebSocketCloseStatus.NormalClosure;
            var closeDescription = "closing";

            while (true)
            {
                var result = await stream.ReadAsync(temp, CancellationToken.None)
                    .ConfigureAwait(false);

                totalBytes += (ulong)result.Count;

                if (maxSize > ulong.MinValue && totalBytes > maxSize)
                {
                    throw new SocketMessageTooLargeException(totalBytes, maxSize);
                }

                // handle close frames
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await stream.CloseAsync(CancellationToken.None);

                    break;
                }

                if (result.Count == 0)
                {
                    break; // end of message
                }

                buffer.Write(temp, 0, result.Count);

                messageType = result.MessageType;
                closeStatus = result.CloseStatus ?? WebSocketCloseStatus.NormalClosure;
                closeDescription = result.CloseStatusDescription ?? "client closed";
            }

            await stream.CompleteAsync(cancellationToken);

            return ParseMessage(stream, messageType, buffer);
        }

        /// <summary>
        /// Parses a WebSocket message from the provided buffer and stream, 
        /// returning a strongly typed socket message instance based on the 
        /// message type and content.
        /// </summary>
        /// <param name="stream">
        /// The stream representing the source of the WebSocket message data. Used 
        /// to provide connection and context information for the resulting message.
        /// </param>
        /// <param name="messageType">
        /// The type of the WebSocket message, indicating whether the message is 
        /// text or binary.
        /// </param>
        /// <param name="buffer">A memory buffer containing the raw message data 
        /// to be parsed. The buffer must be positioned at the start of the message data.
        /// </param>
        /// <returns>
        /// An instance of a class implementing <see cref="ISocketMessage"/> that 
        /// represents the parsed message. The specific type depends on the message 
        /// content and type.
        /// </returns>
        private static ISocketMessage ParseMessage
        (
            ISocketReadStream stream,
            WebSocketMessageType messageType,
            MemoryStream buffer
        )
        {
            // produce SocketMessage from buffer
            buffer.Seek(0, SeekOrigin.Begin);

            if (messageType == WebSocketMessageType.Text)
            {
                // extract UTF‑8 text from the stream
                var text = Encoding.UTF8.GetString(buffer.ToArray());

                ISocketMessage parsed = null;

                try
                {
                    using var doc = JsonDocument.Parse(text);

                    if (doc.RootElement.TryGetProperty("text", out _))
                    {
                        parsed = JsonSerializer.Deserialize<SocketMessageText>(text);
                    }
                    else if (doc.RootElement.TryGetProperty("data", out _))
                    {
                        parsed = JsonSerializer.Deserialize<SocketMessageBinary>(text);
                    }
                    else
                    {
                        // default
                        parsed = JsonSerializer.Deserialize<SocketMessageText>(text);
                    }
                }
                catch
                {
                    // JSON invalid → fallback to plain text message
                    parsed = new SocketMessageText
                    {
                        Type = null,
                        Text = text,
                        ConnectionId = stream.ConnectionId,
                        ApplicationId = stream.SocketContext?.ApplicationContext?.ApplicationId,
                        SocketId = stream.SocketContext?.EndpointId?.ToString()
                    };
                }

                return parsed;
            }
            else // binary
            {
                var bytes = buffer.ToArray();

                return new SocketMessageBinary
                {
                    Type = null,
                    Data = bytes,
                    ConnectionId = stream.ConnectionId,
                    ApplicationId = stream.SocketContext?.ApplicationContext?.ApplicationId,
                    SocketId = stream.SocketContext?.EndpointId?.ToString()
                };
            }
        }
    }
}