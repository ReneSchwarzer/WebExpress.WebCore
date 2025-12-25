using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebSocket.Protocol;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Default implementation of ISocketContext.
    /// this class provides a simple, mutable context object usable for websocket endpoints.
    /// </summary>
    public class SocketContext : ISocketContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns the conditions that must be met for the resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; internal set; } = [];

        /// <summary>
        /// Returns the endpoint id.
        /// </summary>
        public IComponentId EndpointId { get; internal set; }

        /// <summary>
        /// Collection of supported websocket subprotocols.
        /// </summary>
        public IEnumerable<string> SupportedSubProtocols { get; set; }

        /// <summary> 
        /// Returns the default WebSocket message type used by this endpoint when 
        /// sending data. Implementations may choose <see cref="WebSocketMessageType.Text"/> 
        /// for JSON or human-readable content, or <see cref="WebSocketMessageType.Binary"/> 
        /// for binary payloads. 
        /// </summary>
        public SocketMessageType MessageType { get; set; }

        /// <summary>
        /// Maximum allowed message size in bytes, or null when no limit is imposed.
        /// </summary>
        public ulong MaxMessageSize { get; set; }

        /// <summary>
        /// Indicates whether this websocket endpoint requires an authenticated client.
        /// </summary>
        public bool RequiresAuthentication { get; set; }

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache { get; internal set; }

        /// <summary>
        /// Returns or sets whether all subpaths should be taken into sitemap.
        /// </summary>
        public bool IncludeSubPaths { get; internal set; }

        /// <summary>
        /// Returns the internal routing path for the endpoint.
        /// </summary>
        public IRoute Route { get; internal set; }

        /// <summary>
        /// Returns the attributes associated with the page.
        /// </summary>
        public IEnumerable<Attribute> Attributes { get; internal set; }

        /// <summary>
        /// Creates a new instance of DefaultWebSocketContext.
        /// </summary>
        public SocketContext()
        {
        }

        /// <summary>
        /// Returns a short diagnostic representation.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            // return a compact representation with name and supported subprotocols
            var protocols = SupportedSubProtocols != null ? string.Join(",", SupportedSubProtocols) : string.Empty;
            return $"{EndpointId} (protocols: {protocols})";
        }
    }
}