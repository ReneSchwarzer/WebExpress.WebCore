using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using WebExpress.WebCore.Config;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore
{
    /// <summary>
    /// The web server for processing http requests (see RFC 2616). The web server uses Kestrel internally.
    /// </summary>
    public class HttpServer : IHost, II18N, IHttpApplication<HttpContext>
    {
        /// <summary>
        /// Event is triggered after the web server is started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Provides the KestrelServer, which responds to the requests.
        /// </summary>
        private KestrelServer Kestrel { get; set; }

        /// <summary>
        /// Server thread termination.
        /// </summary>
        private CancellationToken ServerToken { get; } = new CancellationToken();

        /// <summary>
        /// Returns or sets the configuration
        /// </summary>
        public HttpServerConfig Config { get; set; }

        /// <summary>
        /// Returns or sets the context.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; protected set; }

        /// <summary>
        /// Returns the culture.
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Returns the execution time of the web server.
        /// </summary>
        public static DateTime ExecutionTime { get; } = DateTime.Now;

        /// <summary>
        /// Returns the request number;
        /// </summary>
        public long RequestNumber { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="context">Der Serverkontext.</param>
        public HttpServer(HttpServerContext context)
        {
            HttpServerContext = new HttpServerContext
            (
                context.Uri,
                context.Endpoints,
                context.PackagePath,
                context.AssetPath,
                context.DataPath,
                context.ConfigPath,
                context.ContextPath,
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
                HttpServerContext.Log.Info(message: this.I18N("webexpress:httpserver.run"));
            }

            if (!HttpListener.IsSupported)
            {
                HttpServerContext.Log.Error(message: this.I18N("webexpress:httpserver.notsupported"));
            }

            var logger = new LogFactory();
            var transportOptions = new OptionsWrapper<SocketTransportOptions>(new SocketTransportOptions());
            var transport = new SocketTransportFactory(transportOptions, logger);
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddMemoryCache();
            serviceCollection.AddLogging(x =>
            {
                x.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                x.AddProvider(logger);
            });
            serviceCollection.AddHttpLogging(x =>
            {
                x.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
            });

            var serverOptions = new OptionsWrapper<KestrelServerOptions>(new KestrelServerOptions()
            {
                AllowSynchronousIO = true,
                AllowResponseHeaderCompression = true,
                AddServerHeader = true,
                ApplicationServices = serviceCollection.BuildServiceProvider()

            });

            serverOptions.Value.Limits.MaxConcurrentConnections = Config?.Limit?.ConnectionLimit > 0 ? Config?.Limit?.ConnectionLimit : serverOptions.Value.Limits.MaxConcurrentConnections;
            serverOptions.Value.Limits.MaxRequestBodySize = Config?.Limit?.UploadLimit > 0 ? Config?.Limit?.UploadLimit : serverOptions.Value.Limits.MaxRequestBodySize;

            foreach (var endpoint in Config.Endpoints)
            {
                AddEndpoint(serverOptions, endpoint);
            }

            Kestrel = new KestrelServer(serverOptions, transport, logger);

            Kestrel.StartAsync(this, ServerToken);

            HttpServerContext.Log.Info(message: this.I18N("webexpress:httpserver.start"), args: [ExecutionTime.ToShortDateString(), ExecutionTime.ToLongTimeString()]);

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

                HttpServerContext.Log.Info(message: this.I18N("webexpress:httpserver.endpoint"), args: endPoint.Uri);

                foreach (var ipAddress in addressList)
                {
                    var ep = new IPEndPoint(ipAddress, port);

                    switch (uri.Scheme)
                    {
                        case "HTTPS": { AddEndpoint(serverOptions, ep, endPoint.PfxFile, endPoint.Password); break; }
                        default: { AddEndpoint(serverOptions, ep); break; }
                    }

                }
            }
            catch (Exception ex)
            {
                HttpServerContext.Log.Error(message: this.I18N("webexpress:httpserver.listen.exeption"), args: endPoint);
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

            HttpServerContext.Log.Info(message: this.I18N("webexpress:httpserver.listen"), args: endPoint.ToString());
        }

        /// <summary>
        /// Adds an endpoint.
        /// </summary>
        /// <param name="serverOptions">The server options.</param>
        /// <param name="pfxFile">The certificate.</param>
        /// <param name="password">The password to the certificate.</param>
        /// <param name="endPoint">The endpoint.</param>
        private void AddEndpoint(OptionsWrapper<KestrelServerOptions> serverOptions, IPEndPoint endPoint, string pfxFile, string password)
        {
            serverOptions.Value.Listen(endPoint, configure =>
            {
                var cert = new X509Certificate2(pfxFile, password);

                configure.UseHttps(cert);
            });

            HttpServerContext.Log.Info(message: this.I18N("webexpress:httpserver.listen"), args: endPoint.ToString());
        }

        /// <summary>
        /// Stops the HTTP(S) server
        /// </summary>
        public void Stop()
        {
            // End running threads
            Kestrel.StopAsync(ServerToken);
        }

        /// <summary>
        /// Handles an incoming request
        /// Concurrent execution
        /// </summary>
        /// <param name="context">The context of the web request.</param>
        /// <returns>The response to be sent back to the caller.</returns>
        private Response HandleClient(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;
            var response = default(Response);
            var culture = request.Culture;
            var uri = request?.Uri;

            HttpServerContext.Log.Debug(message: this.I18N("webexpress:httpserver.connected"), args: context.RemoteEndPoint);
            HttpServerContext.Log.Info(I18N.Translate
            (
                "webexpress:httpserver.request",
                context.RemoteEndPoint,
                ++RequestNumber,
                $"{request?.Method} {request?.Uri} {request?.Protocoll}"
            ));

            // search page in sitemap
            var searchResult = WebEx.ComponentHub.SitemapManager.SearchResource(context.Uri, new SearchContext()
            {
                Culture = culture,
                HttpContext = context,
                HttpServerContext = HttpServerContext
            });

            if (searchResult != null)
            {
                var resourceUri = new UriResource(request.Uri, searchResult.Uri.PathSegments);
                resourceUri = new UriResource(resourceUri, resourceUri.PathSegments, request.Uri.Skip(resourceUri.PathSegments.Count)?.PathSegments)
                {
                    ServerRoot = new UriResource(request.Uri, HttpServerContext.ContextPath.PathSegments),
                    ApplicationRoot = new UriResource(request.Uri, searchResult.EndpointContext?.ModuleContext?.ApplicationContext?.ContextPath.PathSegments),
                    ModuleRoot = new UriResource(request.Uri, searchResult.EndpointContext?.ModuleContext?.ContextPath.PathSegments),
                    ResourceRoot = new UriResource(request.Uri, searchResult.Uri.PathSegments)
                };

                request.Uri = resourceUri;

                try
                {
                    // execute resource
                    request.AddParameter(searchResult.Uri.Parameters.Select(x => new Parameter(x.Key, x.Value, ParameterScope.Url)));

                    if (searchResult.Instance != null)
                    {
                        response = searchResult.Process(request);

                        if (searchResult.Instance is IPage)
                        {
                            response.Content += $"<!-- {stopwatch.ElapsedMilliseconds} ms -->";
                        }

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
                        // Resource not found
                        response = CreateStatusPage<ResponseNotFound>
                        (
                            string.Empty,
                            request,
                            searchResult
                        );
                    }
                }
                catch (RedirectException ex)
                {
                    if (ex.Permanet)
                    {
                        response = new ResponseRedirectPermanentlyMoved(ex.Url);
                    }
                    else
                    {
                        response = new ResponseRedirectTemporarilyMoved(ex.Url);
                    }
                }
                catch (Exception ex)
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
            else
            {
                // Resource not found
                response = CreateStatusPage<ResponseNotFound>(string.Empty, request);
            }

            stopwatch.Stop();

            HttpServerContext.Log.Info(I18N.Translate
            (
                "webexpress:httpserver.request.done",
                context?.RemoteEndPoint,
                RequestNumber,
                stopwatch.ElapsedMilliseconds,
                response.Status
            ));

            return response;
        }

        /// <summary>
        /// Sends the response message
        /// </summary>
        /// <param name="context">The context of the request</param>
        /// <param name="response">The reply message</param>
        /// <returns>Sending the message as a task, which is executed concurrently.</returns>
        private async Task SendResponseAsync(HttpContext context, Response response)
        {
            try
            {
                var responseFeature = context.Features.Get<IHttpResponseFeature>();
                var responseBodyFeature = context.Features.Get<IHttpResponseBodyFeature>();

                responseFeature.StatusCode = response.Status;
                responseFeature.ReasonPhrase = response.Reason;
                responseFeature.Headers.KeepAlive = "true";

                if (response.Header.Location != null)
                {
                    responseFeature.Headers.Location = response.Header.Location;
                }

                if (!string.IsNullOrWhiteSpace(response.Header.CacheControl))
                {
                    responseFeature.Headers.CacheControl = response.Header.CacheControl;
                }

                if (!string.IsNullOrWhiteSpace(response.Header.ContentType))
                {
                    responseFeature.Headers.ContentType = response.Header.ContentType;
                }

                if (response.Header.WWWAuthenticate)
                {
                    responseFeature.Headers.WWWAuthenticate = "Basic realm=\"Bereich\"";
                }

                if (response.Header.Cookies.Count != 0)
                {
                    responseFeature.Headers.SetCookie = string.Join(" ", response.Header.Cookies);
                }

                if (response?.Content is byte[] byteContent)
                {
                    responseFeature.Headers.ContentLength = byteContent.Length;
                    await responseBodyFeature.Stream.WriteAsync(byteContent);
                    await responseBodyFeature.Stream.FlushAsync();
                }
                else if (response?.Content is string strContent)
                {
                    var content = context.Encoding.GetBytes(strContent);

                    responseFeature.Headers.ContentLength = content.Length;
                    await responseBodyFeature.Stream.WriteAsync(content);
                    await responseBodyFeature.Stream.FlushAsync();
                }
                else if (response?.Content is IHtmlNode htmlContent)
                {
                    var content = context.Encoding.GetBytes(htmlContent?.ToString());

                    responseFeature.Headers.ContentLength = content.Length;
                    await responseBodyFeature.Stream.WriteAsync(content);
                    await responseBodyFeature.Stream.FlushAsync();
                }

                responseBodyFeature.Stream.Close();
            }
            catch (Exception ex)
            {
                HttpServerContext.Log.Error(context.RemoteEndPoint + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Creates a status page
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="request">The request.</param>
        /// <param name="searchResult">The plugin by searching the status page or null.</param>
        /// <returns>The response.</returns>
        private Response CreateStatusPage<T>(string message, Request request, SearchResult searchResult = null) where T : Response, new()
        {
            var response = new T() as Response;
            var culture = Culture;

            try
            {
                culture = new CultureInfo(request?.Header?.AcceptLanguage?.FirstOrDefault()?.ToLower());
            }
            catch
            {
            }

            if (searchResult != null)
            {
                return WebEx.ComponentHub.StatusPageManager.CreateStatusResponse
                (
                    message,
                    response.Status,
                    searchResult?.EndpointContext?.ModuleContext?.ApplicationContext,
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
        /// Create an HttpContext with a collection of HTTP features.
        /// </summary>
        /// <param name="contextFeatures">A collection of HTTP features to use to create the HttpContext.</param>
        /// <returns>The HttpContext created.</returns>
        public HttpContext CreateContext(IFeatureCollection contextFeatures)
        {
            try
            {
                return new HttpContext(contextFeatures, HttpServerContext);
            }
            catch (Exception ex)
            {
                return new HttpExceptionContext(ex, contextFeatures);
            }
        }

        /// <summary>
        /// Processes an http context asynchronously.
        /// </summary>
        /// <param name="context">The http context that the operation processes.</param>
        /// <returns>Provides an asynchronous operation that handles the http context.</returns>
        public async Task ProcessRequestAsync(HttpContext context)
        {
            if (context is HttpExceptionContext exceptionContext)
            {
                var message = "<html><head><title>404</title></head><body>" +
                       $"<h4>Message</h4>{exceptionContext.Exception.Message}<br/><br/>" +
                       $"<h5>Source</h5>{exceptionContext.Exception.Source}<br/><br/>" +
                       $"<h5>StackTrace</h5>{exceptionContext.Exception.StackTrace.Replace("\n", "<br/>\n")}<br/><br/>" +
                       $"<h5>InnerException</h5>{exceptionContext.Exception.InnerException?.ToString().Replace("\n", "<br/>\n")}" +
                       "</body></html>";

                var response500 = CreateStatusPage<ResponseInternalServerError>(message, context?.Request);

                await SendResponseAsync(exceptionContext, response500);

                return;
            }

            var response = HandleClient(context);

            await SendResponseAsync(context, response);
        }

        /// <summary>
        /// Discard a specified http context.
        /// </summary>
        /// <param name="context">The http context to discard.</param>
        /// <param name="exception">The exception that is thrown if processing did not complete successfully; otherwise null.</param>
        public void DisposeContext(HttpContext context, Exception exception)
        {
        }
    }
}
