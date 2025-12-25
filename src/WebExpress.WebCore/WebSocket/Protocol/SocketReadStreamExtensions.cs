using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Provides extension methods for reading <see cref="ISocketMessage"/> instances
    /// from an <see cref="ISocketReadStream"/> using the native WebExpress WebSocket protocol.
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
        public static async Task<ISocketMessage> ReadMessageAsync(
            this ISocketReadStream stream,
            CancellationToken cancellationToken = default)
        {
            ulong totalBytes = 0;
            using var buffer = new MemoryStream();
            var temp = new byte[4096];

            var messageType = SocketMessageType.Text;
            var maxSize = stream.SocketContext?.MaxMessageSize ?? ulong.MinValue;

            while (true)
            {
                var result = await stream.ReadAsync(temp, cancellationToken)
                    .ConfigureAwait(false);

                totalBytes += (ulong)result.Count;

                if (maxSize > ulong.MinValue && totalBytes > maxSize)
                {
                    throw new SocketMessageTooLargeException(totalBytes, maxSize);
                }

                if (result.Count == 0)
                {
                    break; // end of message
                }

                buffer.Write(temp, 0, result.Count);
                messageType = result.MessageType;
            }

            await stream.CompleteAsync(cancellationToken);

            return ParseMessage(stream, messageType, buffer);
        }

        /// <summary>
        /// Parses a WebSocket message from the provided buffer and stream,
        /// returning a strongly typed socket message instance based on the
        /// message type and content.
        /// </summary>
        private static ISocketMessage ParseMessage(
            ISocketReadStream stream,
            SocketMessageType messageType,
            MemoryStream buffer)
        {
            buffer.Seek(0, SeekOrigin.Begin);

            if (messageType == SocketMessageType.Text)
            {
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
                        parsed = JsonSerializer.Deserialize<SocketMessageText>(text);
                    }
                }
                catch
                {
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