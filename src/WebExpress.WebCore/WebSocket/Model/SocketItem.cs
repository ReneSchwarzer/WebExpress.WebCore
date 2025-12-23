using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebSocket.Model
{
    /// <summary>
    /// A socket element that contains meta information about a socket endpoint.
    /// </summary>
    internal class SocketItem : IDisposable
    {
        /// <summary>
        /// Returns the endpoint id.
        /// </summary>
        public IComponentId EndpointId { get; internal set; }

        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns or sets the type of socket class.
        /// </summary>
        public Type SocketClass { get; set; }

                /// <summary>
        /// Returns the collection of supported websocket subprotocols.
        /// implementations should return the subprotocol identifiers the endpoint can speak.
        /// </summary>
        public IEnumerable<string> SupportedSubProtocols { get; set; }

        /// <summary> 
        /// Returns the default WebSocket message type used by this endpoint when 
        /// sending data. Implementations may choose <see cref="WebSocketMessageType.Text"/> 
        /// for JSON or human-readable content, or <see cref="WebSocketMessageType.Binary"/> 
        /// for binary payloads. 
        /// </summary>
        public WebSocketMessageType MessageType { get; set; }  

        /// <summary>
        /// Returns the maximum allowed message size in bytes, or null when the endpoint imposes no limit.
        /// servers and hosts may use this to protect against excessively large frames.
        /// </summary>
        public ulong MaxMessageSize { get; set; }

        /// <summary>
        /// Returns the conditions that must be met for the resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; set; }

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache { get; set; }

        /// <summary>
        /// Returns the attributes associated with the socket.
        /// </summary>
        public IEnumerable<Type> Attributes { get; internal set; }

        /// <summary>
        /// Returns the socket context.
        /// note: reuses ISocketContext to remain compatible with existing contexts;
        /// replace with a dedicated socket context type if one exists.
        /// </summary>
        public ISocketContext SocketContext { get; internal set; }

        /// <summary>
        /// Returns or sets the instance of the socket endpoint, if the endpoint is cached, otherwise null.
        /// </summary>
        public IEndpoint Instance { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="endpointManager">The endpoint manager responsible for managing endpoints.</param>
        internal SocketItem(IEndpointManager endpointManager)
        {
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Convert the resource element to a string.
        /// </summary>
        /// <returns>The resource element in its string representation.</returns>
        public override string ToString()
        {
            return $"Socket: '{SocketContext?.EndpointId}'";
        }
    }
}