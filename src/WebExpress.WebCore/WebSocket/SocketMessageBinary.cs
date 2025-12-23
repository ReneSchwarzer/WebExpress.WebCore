using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Represents a WebSocket message containing binary payload.
    /// Implements <see cref="ISocketMessage"/> and provides routing metadata
    /// together with binary content.
    /// </summary>
    public class SocketMessageBinary : ISocketMessage
    {
        /// <inheritdoc />
        public string Type { get; init; }

        /// <summary>
        /// The message identifier for deduplication or request/response correlation.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// The application id this payload belongs to, if applicable.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// The socket id (endpoint id) this payload targets or originates from.
        /// </summary>
        public string SocketId { get; set; }

        /// <summary>
        /// The connection id assigned by the socket manager on registration.
        /// </summary>
        public string ConnectionId { get; set; }

        /// <inheritdoc />
        public string Sender { get; init; }

        /// <inheritdoc />
        public IEnumerable<string> Targets { get; init; }

        /// <inheritdoc />
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;

        /// <inheritdoc />
        public IDictionary<string, string> Meta { get; init; }
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The binary payload of the message.
        /// Automatically Base64-encoded by System.Text.Json.
        /// </summary>
        public byte[] Data { get; init; }

        /// <inheritdoc />
        [JsonIgnore]
        public bool IsBinary => Data?.Length > 0;

        private static readonly JsonSerializerOptions SerializeOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <inheritdoc />
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, SerializeOptions);
        }

        /// <summary>
        /// Creates a new binary message with the specified routing type and payload.
        /// </summary>
        public static SocketMessageBinary Create(string type, byte[] data)
        {
            return new SocketMessageBinary
            {
                Type = type,
                Data = data,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
