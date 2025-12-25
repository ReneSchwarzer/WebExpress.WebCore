using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Represents a WebSocket message containing UTF-8 text payload.
    /// Implements <see cref="ISocketMessage"/> and provides routing metadata
    /// together with human-readable content.
    /// </summary>
    public class SocketMessageText : ISocketMessage
    {
        private static readonly JsonSerializerOptions _serializeOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

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

        /// <summary>
        /// Returns the identifier of the sender associated with this message.
        /// </summary>
        public string Sender { get; init; }

        /// <summary>
        /// Returns the collection of target identifiers associated with this instance.
        /// </summary>
        public IEnumerable<string> Targets { get; init; }

        /// <summary>
        /// Returns the date and time, in Coordinated Universal Time (UTC), 
        /// when the object was created or last updated.
        /// </summary>
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// Returns a collection of key-value pairs that provide additional metadata 
        /// associated with the object.
        /// </summary>
        public IDictionary<string, string> Meta { get; init; }
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The UTF-8 text payload of the message.
        /// </summary>
        public string Text { get; init; }

        /// <summary>
        /// Converts the current object to its JSON string representation.
        /// </summary>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, _serializeOptions);
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
