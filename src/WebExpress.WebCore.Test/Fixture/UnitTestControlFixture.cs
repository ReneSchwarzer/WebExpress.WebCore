using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Globalization;
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
        /// Returns the 
        /// </summary>
        private static List<IResource> Ressources { get; } = new List<IResource>();

        /// <summary>
        /// Initializes a new instance of the class and boot the component manager.
        /// </summary>
        public UnitTestControlFixture()
        {
        }

        /// <summary>
        /// Create a a server context.
        /// </summary>
        /// <returns>The server context.</returns>
        public static IHttpServerContext CreateHttpServerContext()
        {
            return new HttpServerContext
            (
                "localhost",
                [],
                "",
                Environment.CurrentDirectory,
                Environment.CurrentDirectory,
                Environment.CurrentDirectory,
                null,
                CultureInfo.GetCultureInfo("en"),
                new Log() { LogMode = LogMode.Off },
                null
            );
        }

        /// <summary>
        /// Create a component hub.
        /// </summary>
        /// <returns>The component manager.</returns>
        public static ComponentHub CreateComponentHub()
        {
            var ctorComponentManager = typeof(ComponentHub).GetConstructor
            (
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                [typeof(HttpServerContext)],
                null
            );

            var componentManager = (ComponentHub)ctorComponentManager.Invoke([CreateHttpServerContext()]);

            // set static field in the webex class
            var type = typeof(WebEx);
            var field = type.GetField("_componentHub", BindingFlags.Static | BindingFlags.NonPublic);

            field.SetValue(null, componentManager);

            return componentManager;
        }

        /// <summary>
        /// Create a component hub and register the plugins.
        /// </summary>
        /// <returns>The component manager.</returns>
        public static ComponentHub CreateAndRegisterComponentHub()
        {
            var componentManager = CreateComponentHub();
            var pluginManager = componentManager.PluginManager as PluginManager;

            pluginManager.Register();

            return componentManager;
        }

        /// <summary>
        /// Create a fake request.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>A fake request for testing.</returns>
        public static WebMessage.Request CrerateRequest(string content = "")
        {
            var context = CreateHttpContext(content);

            return context.Request;
        }

        /// <summary>
        /// Create a fake http context.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>A fake http context for testing.</returns>
        public static WebMessage.HttpContext CreateHttpContext(string content = "")
        {
            var ctorRequest = typeof(WebMessage.Request).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, [typeof(IFeatureCollection), typeof(RequestHeaderFields), typeof(IHttpServerContext)], null);
            var featureCollection = new FeatureCollection();
            var firstLine = content.Split('\n').FirstOrDefault();
            var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var filteredLines = lines.Skip(1).TakeWhile(line => !string.IsNullOrWhiteSpace(line));
            var pos = content.Length > 0 ? content.IndexOf(filteredLines.LastOrDefault()) + filteredLines.LastOrDefault().Length + 4 : 0;
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
                Body = contentBytes.Length > 0 ? new MemoryStream(contentBytes) : null,
                Method = firstLine.Split(' ')?.Where(x => !string.IsNullOrEmpty(x)).FirstOrDefault() ?? "GET",
                RawTarget = firstLine.Split(' ')?.Skip(1)?.FirstOrDefault()?.Split('?')?.FirstOrDefault() ?? "/",
                QueryString = "?" + firstLine.Split(' ')?.Skip(1)?.FirstOrDefault()?.Split('?')?.Skip(1)?.FirstOrDefault() ?? "",
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

            var componentManager = CreateComponentHub();
            var context = new WebMessage.HttpContext(featureCollection, componentManager.HttpServerContext);

            return context;
        }

        /// <summary>
        /// Create a fake render context.
        /// </summary>
        /// <returns>A fake context for testing.</returns>
        public static RenderContext CrerateContext()
        {
            var request = CrerateRequest();

            return new RenderContext(CreratePageContext()?.ModuleContext?.ApplicationContext, request, []);
        }

        /// <summary>
        /// Create a fake page context.
        /// </summary>
        /// <returns>A fake context for testing.</returns>
        public static PageContext CreratePageContext()
        {
            var ctorPageContext = typeof(PageContext).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, [typeof(IModuleContext)], null);

            var moduleContext = WebEx.ComponentHub.ModuleManager.Modules
                .Where(x => x.ModuleId == typeof(TestModuleA1).FullName.ToLower())
                .FirstOrDefault();

            var pageContext = (PageContext)ctorPageContext.Invoke([moduleContext]);

            return pageContext;
        }

        /// <summary>
        /// Gets the content of an embedded resource as a string.
        /// </summary>
        /// <param name="fileName">The name of the resource file.</param>
        /// <returns>The content of the embedded resource as a string.</returns>
        public static string GetEmbeddedResource(string fileName)
        {
            var assembly = typeof(UnitTestControlFixture).Assembly;
            var resourceName = assembly.GetManifestResourceNames()
                                   .FirstOrDefault(name => name.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
