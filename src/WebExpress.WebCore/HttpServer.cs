using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using WebExpress.WebCore.Config;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebCore.WebSocket;
using WebExpress.WebCore.WebStatusPage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore
{
    /// <summary>
    /// The web server for processing http requests (see RFC 2616). The web server uses Kestrel internally.
    /// </summary>
    public class HttpServer : IHost, IHttpApplication<IHttpContext>
    {
        private static readonly IComponentHub _componentHub = WebEx.ComponentHub;

        /// <summary>
        /// Event is triggered after the web server is started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Provides the KestrelServer, which responds to the requests.
        /// </summary>
        private KestrelServer Kestrel { get; set; }

        /// <summary>
        /// Gets the server thread termination.
        /// </summary>
        private CancellationTokenSource ServerTokenSource { get; } = new CancellationTokenSource();

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        public HttpServerConfig Config { get; set; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; protected set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets the execution time of the web server.
        /// </summary>
        public static DateTime ExecutionTime { get; } = DateTime.Now;

        /// <summary>
        /// Gets the request number;
        /// </summary>
        public long RequestNumber { get; private set; }

        /// <summary>
        /// Gets the statistics history.
        /// </summary>
        public static List<HttpServerStatisticItem> Statistics { get; } = [];

        /// <summary>
        /// Synchronization object for statistics.
        /// </summary>
        private static readonly Lock _statLock = new();

        // Variables for CPU usage calculation
        private static DateTime _lastCpuTime = DateTime.UtcNow;
        private static TimeSpan _lastProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
        private static readonly Process _currentProcess = Process.GetCurrentProcess();

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="context">The server context.</param>
        public HttpServer(IHttpServerContext context)
        {
            HttpServerContext = new HttpServerContext
            (
                context.Route,
                context.Endpoints,
                context.PackagePath,
                context.AssetPath,
                context.DataPath,
                context.ConfigPath,
                context.Culture,
                context.Log,
                this
            );

            Culture = HttpServerContext.Culture;
        }

        /// <summary>
        /// Starts the HTTP(S) server.
        /// </summary>
        public void Start()
        {
            if (HttpServerContext != null && HttpServerContext.Log != null)
            {
                HttpServerContext.Log.Info(message: I18N.Translate("webexpress.webcore:httpserver.run"));
            }

            if (!HttpListener.IsSupported)
            {
                HttpServerContext.Log.Error(message: I18N.Translate("webexpress.webcore:httpserver.notsupported"));
            }

            var logger = new LogFactory();
            var transportOptions = new OptionsWrapper<SocketTransportOptions>
            (
                new SocketTransportOptions()
            );
            var transport = new SocketTransportFactory(transportOptions, logger);
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddMemoryCache();
            serviceCollection.AddLogging(x =>
            {
                x.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                x.AddProvider(logger);
            });
            serviceCollection.AddHttpLogging
            (
                x =>
                {
                    x.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
                }
            );

            var serverOptions = new OptionsWrapper<KestrelServerOptions>(new KestrelServerOptions()
            {
                AllowSynchronousIO = true,
                AllowResponseHeaderCompression = true,
                AddServerHeader = true,
                ApplicationServices = serviceCollection.BuildServiceProvider()
            });

            serverOptions.Value.Limits.MaxConcurrentConnections = Config?.Limit?.ConnectionLimit > 0
                ? Config?.Limit?.ConnectionLimit
                : serverOptions.Value.Limits.MaxConcurrentConnections;
            serverOptions.Value.Limits.MaxRequestBodySize = Config?.Limit?.UploadLimit > 0
                ? Config?.Limit?.UploadLimit
                : serverOptions.Value.Limits.MaxRequestBodySize;

            foreach (var endpoint in Config.Endpoints)
            {
                AddEndpoint(serverOptions, endpoint);
            }

            Kestrel = new KestrelServer(serverOptions, transport, logger);
            Kestrel.StartAsync(this, ServerTokenSource.Token);

            HttpServerContext.Log.Info(message: I18N.Translate
            (
                "webexpress.webcore:httpserver.start"),
                args: [ExecutionTime.ToShortDateString(), ExecutionTime.ToLongTimeString()]
            );

            Started?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Adds an endpoint.
        /// </summary>
        /// <param name="serverOptions">The server options.</param>
        /// <param name="endPoint">The endpoint.</param>
        private void AddEndpoint(OptionsWrapper<KestrelServerOptions> serverOptions, EndpointConfig endPoint)
        {
            try
            {
                var uri = new UriBuilder(endPoint.Uri);
                var asterisk = uri.Host.Equals("*");

                var port = uri.Port;
                var host = asterisk ? Dns.GetHostEntry(Dns.GetHostName()) : Dns.GetHostEntry(uri.Host);
                var addressList = host.AddressList
                    .Union(asterisk ? Dns.GetHostEntry("localhost").AddressList : [])
                    .Where(x => x.AddressFamily == AddressFamily.InterNetwork || x.AddressFamily == AddressFamily.InterNetworkV6);

                HttpServerContext.Log.Info(message: I18N.Translate("webexpress.webcore:httpserver.endpoint"), args: endPoint.Uri);

                foreach (var ipAddress in addressList)
                {
                    var ep = new IPEndPoint(ipAddress, port);

                    switch (uri.Scheme)
                    {
                        case "HTTPS":
                            {
                                AddEndpoint(serverOptions, ep, endPoint.PfxFile, endPoint.Password);
                                break;
                            }
                        default:
                            {
                                AddEndpoint(serverOptions, ep);
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpServerContext.Log.Error(message: I18N.Translate("webexpress.webcore:httpserver.listen.exeption"), args: endPoint);
                HttpServerContext.Log.Exception(ex);
            }
        }

        /// <summary>
        /// Adds an endpoint.
        /// </summary>
        /// <param name="serverOptions">The server options.</param>
        /// <param name="endPoint">The endpoint.</param>
        private void AddEndpoint(OptionsWrapper<KestrelServerOptions> serverOptions, IPEndPoint endPoint)
        {
            serverOptions.Value.Listen(endPoint);
            HttpServerContext.Log.Info(message: I18N.Translate("webexpress.webcore:httpserver.listen"), args: endPoint.ToString());
        }

        /// <summary>
        /// Adds an endpoint with HTTPS configuration.
        /// </summary>
        /// <param name="serverOptions">The server options.</param>
        /// <param name="endPoint">The endpoint.</param>
        /// <param name="pfxFile">The path to the PFX file containing the certificate.</param>
        /// <param name="password">The password for the PFX file.</param>
        private void AddEndpoint(OptionsWrapper<KestrelServerOptions> serverOptions, IPEndPoint endPoint, string pfxFile, string password)
        {
            serverOptions.Value.Listen(endPoint, configure =>
            {
                var cert = X509CertificateLoader.LoadPkcs12FromFile(pfxFile, password, X509KeyStorageFlags.DefaultKeySet);
                configure.UseHttps(cert);
            });

            HttpServerContext.Log.Info(message: I18N.Translate("webexpress.webcore:httpserver.listen"), args: endPoint.ToString());
        }

        /// <summary>
        /// Stops the HTTP(S) server.
        /// </summary>
        public void Stop()
        {
            // signal cancellation and stop server
            ServerTokenSource.Cancel();
            Kestrel.StopAsync(ServerTokenSource.Token);
        }

        /// <summary>
        /// Handles an incoming request.
        /// </summary>
        /// <param name="context">The context of the web request.</param>
        /// <param name="searchResult">The previously resolved search result for the request.</param>
        /// <returns>The response to be sent back to the caller.</returns>
        private IResponse HandleClient(IHttpContext context, SearchResult searchResult)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;
            var response = default(IResponse);

            HttpServerContext.Log.Debug(message: I18N.Translate("webexpress.webcore:httpserver.connected"), args: context.RemoteEndPoint);
            HttpServerContext.Log.Info(I18N.Translate
            (
                "webexpress.webcore:httpserver.request",
                context.RemoteEndPoint,
                ++RequestNumber,
                $"{request?.Method} {request?.Uri} {request?.Protocoll}"
            ));

            var resourceUri = new UriEndpoint(request.Uri, searchResult.Uri.PathSegments);
            request.Uri = resourceUri;

            try
            {
                // execute resource
                request.AddParameter(searchResult.Uri.Parameters.Select(x => new Parameter(x.Key, x.Value, ParameterScope.Url)));

                if (searchResult.EndpointContext != null)
                {
                    response = WebEx.ComponentHub.EndpointManager.HandleRequest(request, searchResult.EndpointContext);

                    if (response is ResponseNotFound)
                    {
                        response = CreateStatusPage<ResponseNotFound>
                        (
                            string.Empty,
                            request,
                            searchResult
                        );
                    }

                    if
                    (
                        !response.Header.Cookies.Where(x => x.Name.Equals("session")).Any() &&
                        !request.Header.Cookies.Where(x => x.Name.Equals("session")).Any() &&
                        request.Session != null
                    )
                    {
                        var cookie = new Cookie("session", request.Session.Id.ToString()) { Expires = DateTime.MaxValue };
                        response.Header.Cookies.Add(cookie);
                    }
                }
                else
                {
                    // resource not found
                    response = CreateStatusPage<ResponseNotFound>
                    (
                        "Resource not found",
                        request,
                        searchResult
                    );
                }
            }
            catch (RedirectException ex)
            {
                if (ex.Permanet)
                {
                    response = new ResponseMovedPermanently(ex.Uri);
                }
                else
                {
                    response = new ResponseMovedTemporarily(ex.Uri);
                }
            }
            catch (BadRequestException ex)
            {
                var message = $"<h4>Message</h4>{ex.Message}<br/><br/>" +
                        $"<h5>Source</h5>{ex.Source}<br/><br/>" +
                        $"<h5>StackTrace</h5>{ex.StackTrace.Replace("\n", "<br/>\n")}";

                response = CreateStatusPage<ResponseBadRequest>
                (
                    message,
                    request,
                    searchResult
                );
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException tie && tie.InnerException is RedirectException rex)
                {
                    response = rex.Permanet
                        ? new ResponseMovedPermanently(rex.Uri)
                        : new ResponseMovedTemporarily(rex.Uri);
                }
                else
                {
                    HttpServerContext.Log.Exception(ex);

                    var message = $"<h4>Message</h4>{ex.Message}<br/><br/>" +
                            $"<h5>Source</h5>{ex.Source}<br/><br/>" +
                            $"<h5>StackTrace</h5>{ex.StackTrace.Replace("\n", "<br/>\n")}<br/><br/>" +
                            $"<h5>InnerException</h5>{ex.InnerException?.ToString().Replace("\n", "<br/>\n")}";

                    response = CreateStatusPage<ResponseInternalServerError>
                    (
                        message,
                        request,
                        searchResult
                    );
                }
            }

            stopwatch.Stop();

            UpdateStatistics(response, stopwatch.ElapsedMilliseconds);

            HttpServerContext.Log.Info(I18N.Translate
            (
                "webexpress.webcore:httpserver.request.done",
                context?.RemoteEndPoint,
                RequestNumber,
                stopwatch.ElapsedMilliseconds,
                response.Status
            ));

            return response;
        }

        /// <summary>
        /// Updates the request statistics with ring buffer logic (max 24h).
        /// </summary>
        /// <param name="response">The response containing the status code.</param>
        /// <param name="duration">The duration of the request in milliseconds.</param>
        private static void UpdateStatistics(IResponse response, long duration)
        {
            var now = DateTime.Now;
            var minute = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            var isError = response != null && response.Status >= 400;

            // calculate memory usage in MB
            var memUsage = _currentProcess.WorkingSet64 / (1024.0 * 1024.0);

            // calculate cpu usage
            var currentCpuTime = _currentProcess.TotalProcessorTime;
            var currentWallTime = DateTime.UtcNow;
            var cpuUsedMs = (currentCpuTime - _lastProcessorTime).TotalMilliseconds;
            var totalMsPassed = (currentWallTime - _lastCpuTime).TotalMilliseconds;
            var cpuUsage = 0.0;

            if (totalMsPassed > 0)
            {
                cpuUsage = (cpuUsedMs / (totalMsPassed * Environment.ProcessorCount)) * 100.0;
            }

            // update pointers for next calculation
            _lastProcessorTime = currentCpuTime;
            _lastCpuTime = currentWallTime;

            lock (_statLock)
            {
                // remove entries older than 24 hours (1440 minutes)
                while (Statistics.Count >= 1440)
                {
                    Statistics.RemoveAt(0);
                }

                var current = Statistics.LastOrDefault();

                if (current != null && current.Timestamp == minute)
                {
                    current.Requests++;
                    if (isError)
                    {
                        current.Errors++;
                    }

                    // update min, max and total duration
                    if (duration < current.MinDuration)
                    {
                        current.MinDuration = duration;
                    }
                    if (duration > current.MaxDuration)
                    {
                        current.MaxDuration = duration;
                    }
                    current.TotalDuration += duration;

                    // calculate moving average for system metrics within this minute
                    current.CpuUsage += (cpuUsage - current.CpuUsage) / current.Requests;
                    current.MemoryUsage += (memUsage - current.MemoryUsage) / current.Requests;
                }
                else
                {
                    Statistics.Add(new HttpServerStatisticItem()
                    {
                        Timestamp = minute,
                        Requests = 1,
                        Errors = isError ? 1 : 0,
                        MinDuration = duration,
                        MaxDuration = duration,
                        TotalDuration = duration,
                        CpuUsage = cpuUsage,
                        MemoryUsage = memUsage
                    });
                }
            }
        }

        /// <summary>
        /// Creates a status page.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="request">The request.</param>
        /// <param name="searchResult">The plugin by searching the status page or null.</param>
        /// <returns>The response.</returns>
        private static IResponse CreateStatusPage<TResponse>(string message, IRequest request, SearchResult searchResult = null)
            where TResponse : Response, new()
        {
            var response = new TResponse() as Response;
            var statusPageManager = WebEx.ComponentHub.StatusPageManager;
            var applicationManager = WebEx.ComponentHub.ApplicationManager;
            var route = new RouteEndpoint(request.Uri.PathSegments)?.ToString();
            var applicationContext = applicationManager.Applications
                   .Where(x => route.StartsWith(x.Route.ToString()))
                   .FirstOrDefault();

            if (searchResult != null)
            {
                return statusPageManager.CreateStatusResponse
                (
                    message,
                    response.Status,
                    searchResult?.EndpointContext?.ApplicationContext,
                    request
                );
            }

            if (applicationContext != null)
            {
                return statusPageManager.CreateStatusResponse
                (
                    message,
                    response.Status,
                    applicationContext,
                    request
                );
            }

            message = $"<html><head><title>{response.Status}</title></head><body>" +
                      $"<p>{message}<br/><p>" +
                      $"</body></html>";

            response.Content = message;
            response.Header.ContentLength = message.Length;
            response.Header.ContentType = "text/html; charset=utf-8";

            return response;
        }

        /// <summary>
        /// Creates an appropriate IHttpContext instance (HttpContext or WebSocketContext) 
        /// based on feature detection.
        /// </summary>
        /// <param name="contextFeatures">The feature collection of the request.</param>
        /// <returns>An IHttpContext instance for the request.</returns>
        public IHttpContext CreateContext(IFeatureCollection contextFeatures)
        {
            try
            {
                var requestFeature = contextFeatures.Get<IHttpRequestFeature>();

                // check if schema or upgrade header indicates websocket
                if (IsWebSocketRequest(requestFeature))
                {
                    // use WebSocketContext for websocket connections
                    return new HttpWebSocketContext(contextFeatures, HttpServerContext);
                }

                // use regular HttpContext for normal HTTP requests
                return new HttpContext(contextFeatures, HttpServerContext);
            }
            catch (Exception ex)
            {
                // fall back to HttpExceptionContext on error
                return new HttpExceptionContext(ex, contextFeatures);
            }
        }

        /// <summary>
        /// Processes an http context asynchronously.
        /// If the request is a websocket upgrade to a configured endpoint, handle 
        /// websocket lifecycle instead of request/response.
        /// Handles missing sitemap endpoints directly here.
        /// </summary>
        /// <param name="httpContext">The http context that the operation processes.</param>
        /// <returns>Provides an asynchronous operation that handles the http context.</returns>
        public async Task ProcessRequestAsync(IHttpContext httpContext)
        {
            var responseSender = new ResponseSender();

            if (httpContext is HttpExceptionContext exceptionContext)
            {
                var message = "<html><head><title>404</title></head><body>" +
                    $"<h4>Message</h4>{exceptionContext.Exception.Message}<br/><br/>" +
                    $"<h5>Source</h5>{exceptionContext.Exception.Source}<br/><br/>" +
                    $"<h5>StackTrace</h5>{exceptionContext.Exception.StackTrace.Replace("\n", "<br/>\n")}<br/><br/>" +
                    $"<h5>InnerException</h5>{exceptionContext.Exception.InnerException?.ToString().Replace("\n", "<br/>\n")}" +
                    "</body></html>";

                var response500 = CreateStatusPage<ResponseInternalServerError>(message, httpContext?.Request);

                await responseSender.SendAsync(exceptionContext, response500);

                return;
            }

            var culture = httpContext?.Request?.Culture;
            var searchResult = WebEx.ComponentHub.SitemapManager.SearchResource(httpContext?.Uri, new SearchContext()
            {
                Culture = culture,
                HttpContext = httpContext,
                HttpServerContext = HttpServerContext
            });

            if (searchResult is null || searchResult.EndpointContext is null)
            {
                var notFoundResponse = CreateStatusPage<ResponseNotFound>
                (
                    "Resource not found",
                    httpContext.Request
                );

                await responseSender.SendAsync(httpContext, notFoundResponse);

                return;
            }

            var applicationContext = searchResult.EndpointContext.ApplicationContext;

            if (httpContext.Request is Request request)
            {
                request.ApplicationContext = applicationContext;
                request.EndpointContext = searchResult.EndpointContext;
            }

            if (httpContext is HttpWebSocketContext)
            {
                // try to obtain websocket context and optional handler
                var socketContext = searchResult.EndpointContext as ISocketContext;

                await HandleWebSocketAsync(httpContext, socketContext);

                return;
            }

            // no policies
            if (!searchResult.EndpointContext.Policies?.Any() ?? false)
            {
                var response = HandleClient(httpContext, searchResult);
                await responseSender.SendAsync(httpContext, response);

                return;
            }

            var identity = _componentHub.IdentityManager.GetCurrentIdentity(httpContext.Request);

            // if access is granted
            if (_componentHub.IdentityManager.CheckAccess(identity, searchResult.EndpointContext))
            {
                var response = HandleClient(httpContext, searchResult);
                await responseSender.SendAsync(httpContext, response);

                return;
            }

            // try to authenticate if no identity exists
            if (identity is null)
            {
                identity = _componentHub.IdentityManager.Authenticate(httpContext.Request, applicationContext);
                _componentHub.IdentityManager.Login(httpContext.Request, identity);
            }

            // check again
            if (!_componentHub.IdentityManager.CheckAccess(identity, searchResult.EndpointContext))
            {
                // if the user is authenticated but lacks the required permissions, show the forbidden page
                if (identity is not null)
                {
                    var forbiddenResponse = _componentHub.IdentityManager.CreateForbiddenResponse
                    (
                        httpContext.Request,
                        searchResult.EndpointContext,
                        identity
                    );

                    if (forbiddenResponse is not null)
                    {
                        await responseSender.SendAsync(httpContext, forbiddenResponse);
                        return;
                    }
                }

                // if the user is not authenticated, show the login prompt
                var loginResponse = _componentHub.IdentityManager.CreateAuthenticationPrompt
                (
                    httpContext.Request,
                    searchResult.EndpointContext,
                    identity
                );

                if (loginResponse is not null)
                {
                    await responseSender.SendAsync(httpContext, loginResponse);
                    return;
                }
            }

            // access is granted
            {
                var response = HandleClient(httpContext, searchResult);
                await responseSender.SendAsync(httpContext, response);
            }
        }

        /// <summary>
        /// Handles the complete WebSocket request lifecycle for the given HTTP context.
        /// Validates the upgrade request, delegates the connection handling to the socket manager,
        /// and returns appropriate HTTP error responses when the handshake or connection setup fails.
        /// </summary>
        /// <param name="httpContext">
        /// The current HTTP context containing the incoming WebSocket upgrade request.
        /// </param>
        /// <param name="socketContext">
        /// Optional WebSocket endpoint context resolved from the sitemap. May be <c>null</c>
        /// if the endpoint does not define additional metadata.
        /// </param>
        public async Task HandleWebSocketAsync(IHttpContext httpContext, ISocketContext socketContext)
        {
            var responseSender = new ResponseSender();
            var socketManager = WebEx.ComponentHub.SocketManager;

            // validate that the request is a websocket upgrade
            if (httpContext is not HttpWebSocketContext)
            {
                // websocket not requested by client; return 400 Bad Request
                await responseSender.SendAsync(httpContext, new ResponseBadRequest(new StatusMessage("WebSocket upgrade required")));

                return;
            }

            try
            {
                await socketManager.HandleConnectionAsync(httpContext, socketContext);
            }
            catch (SocketHandshakeException)
            {
                // missing or invalid websocket handshake headers -> respond with 426
                var response = new ResponseUpgradeRequired(new StatusMessage("Invalid WebSocket handshake headers"));
                response.Header.Upgrade = "websocket";

                await responseSender.SendAsync(httpContext, response);
            }
            catch (SocketMessageTooLargeException ex)
            {
                // the client sent a WebSocket message exceeding the configured maximum size -> respond with 413 
                var response = new ResponsePayloadTooLarge(new StatusMessage($"WebSocket message exceeds the maximum allowed size of {ex.MaxSize} bytes."));
                await responseSender.SendAsync(httpContext, response);
            }
            catch (SocketException ex)
            {
                HttpServerContext.Log.Exception(ex);

                // return 500 when socket error 
                var response = new ResponseInternalServerError(new StatusMessage("A transport-level socket error occurred during WebSocket communication."));
                await responseSender.SendAsync(httpContext, response);
            }
            catch (Exception ex)
            {
                // log unhandled exceptions during websocket processing
                HttpServerContext.Log.Exception(ex);

                // return 500 when handshake did not succeed and no websocket established
                var response = new ResponseInternalServerError(new StatusMessage("An unexpected server error occurred during WebSocket processing."));
                await responseSender.SendAsync(httpContext, response);
            }
        }

        /// <summary>
        /// Discard a specified http context.
        /// </summary>
        /// <param name="context">The http context to discard.</param>
        /// <param name="exception">The exception that is thrown if processing did not complete successfully; otherwise null.</param>
        public void DisposeContext(IHttpContext context, Exception exception)
        {
        }

        /// <summary>
        /// Checks whether the current request is a WebSocket connection.
        /// </summary>
        /// <param name="requestFeature">The HTTP request feature instance.</param>
        /// <returns>True if it is a WebSocket connection; otherwise, false.</returns>
        private static bool IsWebSocketRequest(IHttpRequestFeature requestFeature)
        {
            // check scheme and "Upgrade" header for websocket protocol
            if (requestFeature == null)
            {
                return false;
            }

            var upgradeHeader = requestFeature.Headers.Upgrade;
            var scheme = requestFeature.Scheme;
            var isWebSocket =
                upgradeHeader.Contains("websocket", StringComparer.OrdinalIgnoreCase) ||
                string.Equals(scheme, "ws", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(scheme, "wss", StringComparison.OrdinalIgnoreCase);

            return isWebSocket;
        }
    }
}