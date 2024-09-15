using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Net;
using System.Reflection;
using System.Text;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test.Fixture
{
    public class UnitTestControlFixture : IDisposable
    {
        /// <summary>
        /// Returns a guard to protect against concurrent access.
        /// </summary>
        private static object guard = new object();

        /// <summary>
        /// Initializes a new instance of the class and boot the component manager.
        /// </summary>
        public UnitTestControlFixture()
        {
            lock (guard)
            {
                var initializationComponentManager = typeof(ComponentManager).GetMethod("Initialization", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, [typeof(IHttpServerContext)]);
                var serverContext = new HttpServerContext
                (
                    "localhost",
                    [],
                    "",
                    Environment.CurrentDirectory,
                    Environment.CurrentDirectory,
                    Environment.CurrentDirectory,
                    null,
                    null,
                    new Log() { LogMode = LogMode.Off },
                    null
                );

                initializationComponentManager.Invoke(null, [serverContext]);
            }
        }

        /// <summary>
        /// Register a plugin.
        /// </summary>
        /// <param name="plugin">The plugin to be registered.</param>
        public void RegisterPlugin(Type plugin)
        {
            lock (guard)
            {
                var registerPluginManager = typeof(PluginManager).GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Instance, [typeof(Assembly), typeof(PluginLoadContext)]);

                registerPluginManager.Invoke(ComponentManager.PluginManager, [plugin.Assembly, null]);
            }
        }

        /// <summary>
        /// Create a fake request.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>A fake request for testing.</returns>
        public WebMessage.Request CrerateRequest(string content = "")
        {
            var ctorRequestHeaderFields = typeof(RequestHeaderFields).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, [typeof(IFeatureCollection)], null);
            var ctorRequest = typeof(WebMessage.Request).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, [typeof(IFeatureCollection), typeof(IHttpServerContext), typeof(RequestHeaderFields)], null);
            var featureCollection = new FeatureCollection();
            var firstLine = content.Split('\n').FirstOrDefault();
            var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var filteredLines = lines.Skip(1).TakeWhile(line => !string.IsNullOrWhiteSpace(line));
            var pos = content.IndexOf(filteredLines.LastOrDefault()) + filteredLines.LastOrDefault().Length + 4;
            var innerContent = pos < content.Length ? content.Substring(pos) : "";
            var contentBytes = Encoding.UTF8.GetBytes(innerContent);

            var requestFeature = new HttpRequestFeature
            {
                Headers = new HeaderDictionary
                {
                    ["Host"] = "localhost",
                    ["Connection"] = "keep-alive",
                    ["ContentType"] = "text/html",
                    ["ContentLength"] = innerContent.Length.ToString(),
                    ["ContentLanguage"] = "en",
                    ["ContentEncoding"] = "gzip, deflate, br, zstd",
                    ["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7",
                    ["AcceptEncoding"] = "gzip, deflate, br, zstd",
                    ["AcceptLanguage"] = "de,de-DE;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6",
                    ["UserAgent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36 Edg/126.0.0.0",
                    ["Referer"] = "0HN50661TV8TP"
                },
                Body = new MemoryStream(contentBytes),
                Method = firstLine.Split(' ').FirstOrDefault(),
                RawTarget = firstLine.Split(' ').Skip(1).FirstOrDefault().Split('?').FirstOrDefault(),
                QueryString = "?" + firstLine.Split(' ').Skip(1).FirstOrDefault().Split('?').Skip(1).FirstOrDefault(),
            };

            foreach (var line in filteredLines)
            {
                var key = line.Split(':').FirstOrDefault().Trim();
                var value = line.Split(':').Skip(1).FirstOrDefault().Trim();
                requestFeature.Headers[key] = value;
            }

            requestFeature.Headers.ContentLength = contentBytes.Length;

            var requestIdentifierFeature = new HttpRequestIdentifierFeature
            {
                TraceIdentifier = "Ihr TraceIdentifier-Wert"
            };

            var connectionFeature = new HttpConnectionFeature
            {
                LocalPort = 8080,
                LocalIpAddress = IPAddress.Parse("192.168.0.1"),
                RemotePort = 8080,
                RemoteIpAddress = IPAddress.Parse("127.0.0.1"),
                ConnectionId = "0HN50661TV8TP"
            };

            featureCollection.Set<IHttpRequestFeature>(requestFeature);
            featureCollection.Set<IHttpRequestIdentifierFeature>(requestIdentifierFeature);
            featureCollection.Set<IHttpConnectionFeature>(connectionFeature);

            var serverContext = new HttpServerContext
            (
                "localhost",
                [],
                "",
                "",
                "",
                "",
                null,
                null,
                new Log() { LogMode = LogMode.Off },
                null
            );
            var headers = (RequestHeaderFields)ctorRequestHeaderFields.Invoke([featureCollection]);
            var request = (WebMessage.Request)ctorRequest.Invoke([featureCollection, serverContext, headers]);

            return request;
        }

        /// <summary>
        /// Create a fake render context.
        /// </summary>
        /// <returns>A fake context for testing.</returns>
        public RenderContext CrerateContext()
        {
            var request = CrerateRequest();
            var page = new TestPage();
            var visualTree = new VisualTree();

            page.Initialization(CrerateResourceContext());

            return new RenderContext(page, request, visualTree);
        }

        /// <summary>
        /// Create a fake resource context.
        /// </summary>
        /// <returns>A fake context for testing.</returns>
        public ResourceContext CrerateResourceContext()
        {
            var ctorResourceContext = typeof(ResourceContext).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, [typeof(IModuleContext)], null);

            var moduleContext = ComponentManager.ModuleManager.Modules
                .Where(x => x.ModuleId == typeof(TestModule).FullName.ToLower())
                .FirstOrDefault();

            var resourceContext = (ResourceContext)ctorResourceContext.Invoke([moduleContext]);

            return resourceContext;
        }

        /// <summary>
        /// Gets the content of an embedded resource as a string.
        /// </summary>
        /// <param name="fileName">The name of the resource file.</param>
        /// <returns>The content of the embedded resource as a string.</returns>
        public string GetEmbeddedResource(string fileName)
        {
            var assembly = GetType().Assembly;
            var resourceName = assembly.GetManifestResourceNames()
                                   .FirstOrDefault(name => name.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
