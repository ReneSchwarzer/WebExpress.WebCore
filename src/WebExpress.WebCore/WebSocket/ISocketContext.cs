using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using WebExpress.WebCore.WebEndpoint;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Defines the context for a WebSocket endpoint, providing access to configuration and metadata
    /// that are relevant during the websocket lifecycle.
    /// </summary>
    public interface ISocketContext : IEndpointContext
    {
        /// <summary>
        /// Returns the collection of supported websocket subprotocols.
        /// implementations should return the subprotocol identifiers the endpoint can speak.
        /// </summary>
        IEnumerable<string> SupportedSubProtocols { get; }

        /// <summary> 
        /// Returns the default WebSocket message type used by this endpoint when 
        /// sending data. Implementations may choose <see cref="WebSocketMessageType.Text"/> 
        /// for JSON or human-readable content, or <see cref="WebSocketMessageType.Binary"/> 
        /// for binary payloads. 
        /// </summary>
        WebSocketMessageType MessageType { get; }  

        /// <summary>
        /// Returns the maximum allowed message size in bytes, or null when the endpoint imposes no limit.
        /// servers and hosts may use this to protect against excessively large frames.
        /// </summary>
        ulong MaxMessageSize { get; }

        /// <summary>
        /// Indicates whether this websocket endpoint requires an authenticated client.
        /// the host can check this and reject upgrades when authentication is missing.
        /// </summary>
        bool RequiresAuthentication { get; }
    }
}