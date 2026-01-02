using Microsoft.AspNetCore.Http.Features;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a request for a WebSocket connection.
    /// </summary>
    public class RequestWebSocket : RequestBase
    {
        /// <summary>
        /// Initializes a new instance for a WebSocket request. 
        /// Use this after WebSocket handshake is established.
        /// </summary>
        /// <param name="contextFeatures">The feature collection from ASP.NET Core.</param>
        /// <param name="header">The parsed header fields of the request.</param>
        /// <param name="httpServerContext">The context of the web server.</param>
        internal RequestWebSocket(IFeatureCollection contextFeatures, RequestHeaderFields header, IHttpServerContext httpServerContext)
            : base(contextFeatures, header, httpServerContext)
        {
        }

        /// <summary>
        /// Parse the request parameters.
        /// </summary>
        protected override void ParseRequestParams()
        {
        }
    }
}