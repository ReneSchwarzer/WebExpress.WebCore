using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebSession.Model;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a request for a WebSocket connection.
    /// </summary>
    public class RequestWebSocket : IRequest
    {
        private readonly ParameterDictionary _param = [];

        /// <summary>
        /// The context of the web server.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; protected set; }

        /// <summary>
        /// Returns the request method (typically GET for WebSocket handshake).
        /// </summary>
        public RequestMethod Method { get; private set; }

        /// <summary>
        /// Returns the URI.
        /// </summary>
        public UriEndpoint Uri { get; set; }

        /// <summary>
        /// Returns the session.
        /// </summary>
        public Session Session { get; private set; }

        /// <summary>
        /// Returns the HTTP version.
        /// </summary>
        public string Protocoll { get; private set; }

        /// <summary>
        /// Returns the header fields.
        /// </summary>
        public RequestHeaderFields Header { get; private set; }

        /// <summary>
        /// Returns the server's local endpoint.
        /// </summary>
        public EndPoint LocalEndPoint { get; private set; }

        /// <summary>
        /// Returns the client's remote endpoint.
        /// </summary>
        public EndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// Indicates whether the connection is secured (wss).
        /// </summary>
        public bool IsSecureConnection { get; private set; }

        /// <summary>
        /// Returns the scheme (ws or wss).
        /// </summary>
        public UriScheme Scheme { get; private set; }

        /// <summary>
        /// Returns the request identifier.
        /// </summary>
        public string RequestTraceIdentifier { get; private set; }

        /// <summary>
        /// Returns the culture.
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                try
                {
                    var languages = Header?.AcceptLanguage?.FirstOrDefault();
                    var language = languages?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
                    return new CultureInfo(language);
                }
                catch
                {
                    return HttpServerContext.Culture ?? CultureInfo.CurrentCulture;
                }
            }
        }

        /// <summary>
        /// Returns the current WebSocket message type.
        /// </summary>
        public string WebSocketMessageType { get; internal set; }

        /// <summary>
        /// Returns true, if the WebSocket is open.
        /// </summary>
        public bool IsWebSocketOpen { get; internal set; }

        /// <summary>
        /// Initializes a new instance for a WebSocket request. 
        /// Use this after WebSocket handshake is established.
        /// </summary>
        /// <param name="httpServerContext">The server context.</param>
        /// <param name="uri">The endpoint URI.</param>
        /// <param name="session">The session.</param>
        /// <param name="header">Header fields.</param>
        /// <param name="method">Method (typically GET at handshake).</param>
        /// <param name="protocoll">HTTP version.</param>
        /// <param name="scheme">The URI scheme (ws, wss).</param>
        /// <param name="localEndPoint">The local endpoint.</param>
        /// <param name="remoteEndPoint">The remote endpoint.</param>
        /// <param name="traceId">Trace identifier.</param>
        /// <param name="isSecureConnection">Whether the connection is secure.</param>
        internal RequestWebSocket
        (
            IHttpServerContext httpServerContext,
            UriEndpoint uri,
            Session session,
            RequestHeaderFields header,
            RequestMethod method,
            string protocoll,
            UriScheme scheme,
            EndPoint localEndPoint,
            EndPoint remoteEndPoint,
            string traceId,
            bool isSecureConnection
        )
        {
            HttpServerContext = httpServerContext;
            Uri = uri;
            Session = session;
            Header = header;
            Method = method;
            Protocoll = protocoll;
            Scheme = scheme;
            LocalEndPoint = localEndPoint;
            RemoteEndPoint = remoteEndPoint;
            RequestTraceIdentifier = traceId;
            IsSecureConnection = isSecureConnection;

            // WebSocket specific defaults
            WebSocketMessageType = null;
            IsWebSocketOpen = true;

            ParseSessionParams();
        }

        /// <summary>
        /// Adds several parameters.
        /// </summary>
        /// <param name="param">The parameters.</param>
        public void AddParameter(IEnumerable<Parameter> param)
        {
            foreach (var p in param)
            {
                AddParameter(p);
            }
        }

        /// <summary>
        /// Adds one parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public void AddParameter(Parameter param)
        {
            var key = param.Key.ToLower();

            if (!_param.TryAdd(key, param))
            {
                _param[key] = param;
            }
        }

        /// <summary>
        /// Returns a parameter by name.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>The value.</returns>
        public IParameter GetParameter(string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && HasParameter(name))
            {
                return _param[name.ToLower()];
            }

            return null;
        }

        /// <summary>
        /// Returns a parameter by type.
        /// </summary>
        /// <typeparam name="TParameter">The parameter type.</typeparam>
        /// <returns>The value.</returns>
        public IParameter GetParameter<TParameter>()
            where TParameter : IParameter
        {
            var parameter = Parameter.GetParameter<TParameter>();
            if (parameter is not null
                && !string.IsNullOrWhiteSpace(parameter.Key)
                && HasParameter(parameter.Key))
            {
                var p = _param[parameter.Key.ToLower()];
                parameter.Value = p.Value;
                parameter.Scope = p.Scope;

                return parameter;
            }

            return null;
        }

        /// <summary>
        /// Checks whether a parameter exists.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>True if the parameter is present, false otherwise.</returns>
        public bool HasParameter(string name)
        {
            if (name is null)
            {
                return false;
            }

            return _param.ContainsKey(name.ToLower());
        }

        /// <summary>
        /// Parse the session parameters.
        /// </summary>
        private void ParseSessionParams()
        {
            Session = WebEx.ComponentHub?.SessionManager?.GetSession(this);

            var property = Session?.GetProperty<SessionPropertyParameter>();
            if (property is not null && property.Params is not null)
            {
                foreach (var param in property.Params)
                {
                    AddParameter(new Parameter(param.Key?.ToLower(), param.Value.Value, ParameterScope.Session));
                }
            }
        }
    }
}