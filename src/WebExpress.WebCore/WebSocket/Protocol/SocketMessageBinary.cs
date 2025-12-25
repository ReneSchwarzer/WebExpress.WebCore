using System;
using System.Collections.Generic;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Represents a WebSocket message containing binary payload.
    /// Implements <see cref="ISocketMessage"/> and provides routing metadata
    /// together with binary content.
    /// </summary>
    public class SocketMessageBinary : ISocketMessage
    {
        /// <summary>
        /// Returns the type identifier associated with the current instance.
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
        /// Returns the date and time when the object was created or last updated, 
        /// in Coordinated Universal Time (UTC).
        /// </summary>
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// Returns a collection of key-value pairs that provide additional 
        /// metadata associated with the object.
        /// </summary>
        public IDictionary<string, string> Meta { get; init; }
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The binary payload of the message.
        /// Automatically Base64-encoded by System.Text.Json.
        /// </summary>
        public byte[] Data { get; init; }

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
