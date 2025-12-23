using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Base class for structured WebSocket messages exchanged between client and server.
    /// Contains routing metadata common to all message types.
    /// </summary>
    public interface ISocketMessage
    {
        /// <summary>
        /// Application-defined message type used for routing.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// The message identifier for deduplication or request/response correlation.
        /// </summary>
        string MessageId { get; }

        /// <summary>
        /// The application id this payload belongs to, if applicable.
        /// </summary>
        string ApplicationId { get; }

        /// <summary>
        /// The socket id (endpoint id) this payload targets or originates from.
        /// </summary>
        string SocketId { get; }

        /// <summary>
        /// The connection id assigned by the socket manager on registration.
        /// </summary>
        string ConnectionId { get; }

        /// <summary>
        /// Optional sender identifier.
        /// </summary>
        string Sender { get; }

        /// <summary>
        /// Optional list of target identifiers.
        /// </summary>
        IEnumerable<string> Targets { get; }

        /// <summary>
        /// Timestamp in UTC when the message was created.
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Arbitrary metadata as key/value pairs.
        /// </summary>
        IDictionary<string, string> Meta { get; }

        /// <summary>
        /// Indicates whether this message contains binary payload.
        /// </summary>
        [JsonIgnore]
        abstract bool IsBinary { get; }

        /// <summary>
        /// Serializes the message to JSON.
        /// </summary>
        /// <returns> 
        /// A JSON string containing the serialized form of the message, including 
        /// routing metadata and payload fields. 
        /// </returns>
        string ToJson();
    }
}
