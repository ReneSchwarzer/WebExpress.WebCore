using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Represents a WebSocket message containing UTF-8 text payload.
    /// Implements <see cref="ISocketMessage"/> and provides routing metadata
    /// together with human-readable content.
    /// </summary>
    public class SocketMessageText : ISocketMessage
    {
        /// <summary>
        /// Application-defined message type used for routing.
        /// </summary>
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
        /// The UTF-8 text payload of the message.
        /// </summary>
        public string Text { get; init; }

        /// <inheritdoc />
        [JsonIgnore]
        public bool IsBinary => false;

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
        /// Creates a new text message with the specified routing type and payload.
        /// </summary>
        public static SocketMessageText Create(string type, string text)
        {
            return new SocketMessageText
            {
                Type = type,
                Text = text,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
