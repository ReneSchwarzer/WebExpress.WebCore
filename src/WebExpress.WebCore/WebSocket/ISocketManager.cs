using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// The socket manager manages socket endpoints (see RFC 6455 – The WebSocket Protocol) which can be called with a URI.
    /// </summary>
    public interface ISocketManager : IComponentManager
    {
        /// <summary>
        /// An event that fires when a websocket context is added.
        /// </summary>
        event EventHandler<ISocketContext> AddSocket;

        /// <summary>
        /// An event that fires when a websocket context is removed.
        /// </summary>
        event EventHandler<ISocketContext> RemoveSocket;

        /// <summary>
        /// Returns all registered websocket contexts.
        /// </summary>
        IEnumerable<ISocketContext> Sockets { get; }

        /// <summary>
        /// Returns an enumeration of all websocket contexts provided by a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose sockets are to be returned.</param>
        /// <returns>An enumeration of websocket contexts.</returns>
        IEnumerable<ISocketContext> GetSockets(IPluginContext pluginContext);

        /// <summary>
        /// Returns an enumeration of websocket contexts filtered by endpoint type.
        /// </summary>
        /// <typeparam name="T">The websocket endpoint type.</typeparam>
        /// <returns>An enumeration of websocket contexts.</returns>
        IEnumerable<ISocketContext> GetSockets<T>()
            where T : ISocket;

        /// <summary>
        /// Returns an enumeration of websocket contexts filtered by endpoint type.
        /// </summary>
        /// <param name="socketType">The websocket endpoint type.</param>
        /// <returns>An enumeration of websocket contexts.</returns>
        IEnumerable<ISocketContext> GetSockets(Type socketType);

        /// <summary>
        /// Returns an enumeration of websocket contexts filtered by endpoint type and application context.
        /// </summary>
        /// <param name="socketType">The websocket endpoint type.</param>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of websocket contexts.</returns>
        IEnumerable<ISocketContext> GetSockets(Type socketType, IApplicationContext applicationContext);

        /// <summary>
        /// Returns an enumeration of websocket contexts filtered by endpoint type and application context.
        /// </summary>
        /// <typeparam name="T">The websocket endpoint type.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of websocket contexts.</returns>
        IEnumerable<ISocketContext> GetSockets<T>(IApplicationContext applicationContext) where T : ISocket;

        /// <summary>
        /// Returns the websocket context by application context and socket id.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="socketId">The socket id.</param>
        /// <returns>A websocket context or null.</returns>
        ISocketContext GetSocket(IApplicationContext applicationContext, string socketId);

        /// <summary>
        /// Returns the websocket context by application id and socket id.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="socketId">The socket id.</param>
        /// <returns>A websocket context or null.</returns>
        ISocketContext GetSocket(string applicationId, string socketId);

        /// <summary> 
        /// Handles the complete lifecycle of a WebSocket connection for the given HTTP context. 
        /// Performs the server-side upgrade, invokes connection callbacks, processes incoming 
        /// WebSocket frames, parses messages, dispatches them to the socket handler, and triggers 
        /// disconnect and error callbacks as required. 
        /// </summary> 
        /// <param name="httpContext"> 
        /// The current HTTP context containing the WebSocket upgrade request and connection metadata. 
        /// </param> 
        /// <param name="socketContext"> 
        /// Context information for the WebSocket endpoint, including supported subprotocols 
        /// and application-level metadata. 
        /// </param> 
        /// <returns> 
        /// A task that represents the asynchronous handling of the WebSocket connection. 
        /// </returns>
        Task HandleConnectionAsync(IHttpContext httpContext, ISocketContext socketContext);
    }
}