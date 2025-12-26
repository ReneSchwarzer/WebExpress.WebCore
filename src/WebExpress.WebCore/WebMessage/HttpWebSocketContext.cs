using Microsoft.AspNetCore.Http.Features;
using System;
using System.Net;
using System.Text;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents the context for a WebSocket connection.
    /// </summary>
    public class HttpWebSocketContext : IHttpContext
    {
        /// <summary>
        /// Returns the context of the web server.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; protected set; }

        /// <summary>
        /// Returns the context id.
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// Returns the request associated with this context.
        /// </summary>
        public IRequest Request { get; protected set; }

        /// <summary>
        /// Returns the ip address and port number of the server receiving the request.
        /// </summary>
        public EndPoint LocalEndPoint { get; protected set; }

        /// <summary>
        /// Returns the ip address and port number of the client making the request.
        /// </summary>
        public EndPoint RemoteEndPoint { get; protected set; }

        /// <summary>
        /// Returns the set of features for this context.
        /// </summary>
        public IFeatureCollection Features { get; protected set; }

        /// <summary>
        /// Returns the encoding used by this context.
        /// </summary>
        public Encoding Encoding { get; protected set; } = Encoding.Default;

        /// <summary>
        /// Returns the URI associated with this context.
        /// </summary>
        public Uri Uri { get; internal set; }

        /// <summary>
        /// Returns the WebSocket key for this context.
        /// </summary>
        public string WebSocketKey { get; protected set; }

        /// <summary>
        /// Returns whether this WebSocket is secure (wss).
        /// </summary>
        public bool IsSecureWebSocket { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the WebSocketContext class.
        /// </summary>
        /// <param name="contextFeatures">The initial set of features.</param>
        /// <param name="httpServerContext">The context of the web server.</param>
        public HttpWebSocketContext(IFeatureCollection contextFeatures, IHttpServerContext httpServerContext)
        {
            var connectionFeature = contextFeatures.Get<IHttpConnectionFeature>();
            var requestFeature = contextFeatures.Get<IHttpRequestFeature>();
            var header = new RequestHeaderFields(contextFeatures);
            var baseUri = new UriBuilder(requestFeature.Scheme, header.Host, connectionFeature.LocalPort).Uri;

            Features = contextFeatures;
            HttpServerContext = httpServerContext;
            Id = connectionFeature.ConnectionId;
            LocalEndPoint = new IPEndPoint(connectionFeature.LocalIpAddress, connectionFeature.LocalPort);
            RemoteEndPoint = new IPEndPoint(connectionFeature.RemoteIpAddress, connectionFeature.RemotePort);

            Encoding = requestFeature.Headers.ContentEncoding.Count != 0
                ? Encoding.GetEncoding(requestFeature.Headers.ContentEncoding)
                : Encoding.Default;
            Uri = new Uri(baseUri, requestFeature.RawTarget);

            // always initialize as websocket-request for this context
            Request = new RequestWebSocket(contextFeatures, header, httpServerContext);

            WebSocketKey = requestFeature.Headers["Sec-WebSocket-Key"];
            IsSecureWebSocket = requestFeature.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);
        }
    }
}