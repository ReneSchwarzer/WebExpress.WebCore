using System.Text;
using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebAsset;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebSitemap;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the asset manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestAssetManager
    {
        /// <summary>
        /// Test the register function of the asset manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(12, componentHub.AssetManager.Assets.Count());
        }

        /// <summary>
        /// Test the remove function of the asset manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var resourceManager = componentHub.AssetManager as AssetManager;

            // test execution
            resourceManager.Remove(plugin);

            Assert.Empty(componentHub.AssetManager.Assets);
        }

        /// <summary>
        /// Test the id property of the asset.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "webexpress.webcore.test.css.mycss.css")]
        [InlineData(typeof(TestApplicationA), "webexpress.webcore.test.css.my-css.css")]
        [InlineData(typeof(TestApplicationA), "webexpress.webcore.test.js.myjavascript.js")]
        [InlineData(typeof(TestApplicationA), "webexpress.webcore.test.js.myjavascript.mini.js")]
        [InlineData(typeof(TestApplicationB), "webexpress.webcore.test.css.mycss.css")]
        [InlineData(typeof(TestApplicationB), "webexpress.webcore.test.js.myjavascript.js")]
        [InlineData(typeof(TestApplicationB), "webexpress.webcore.test.js.myjavascript.mini.js")]
        [InlineData(typeof(TestApplicationC), "webexpress.webcore.test.css.mycss.css")]
        [InlineData(typeof(TestApplicationC), "webexpress.webcore.test.js.myjavascript.js")]
        [InlineData(typeof(TestApplicationC), "webexpress.webcore.test.js.myjavascript.mini.js")]
        public void Id(Type applicationType, string id)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var asset = componentHub.AssetManager.GetAssets(application)?.FirstOrDefault(x => x.EndpointId.ToString() == id);

            // test execution
            Assert.Equal(id, asset?.EndpointId.ToString());
        }

        /// <summary>
        /// Test the uri property of the asset.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/server/appa/assets/css/mycss.css")]
        [InlineData(typeof(TestApplicationA), "/server/appa/assets/css/my-css.css")]
        [InlineData(typeof(TestApplicationA), "/server/appa/assets/js/myjavascript.js")]
        [InlineData(typeof(TestApplicationA), "/server/appa/assets/js/myjavascript.mini.js")]
        [InlineData(typeof(TestApplicationB), "/server/appb/assets/css/mycss.css")]
        [InlineData(typeof(TestApplicationB), "/server/appb/assets/js/myjavascript.js")]
        [InlineData(typeof(TestApplicationB), "/server/appb/assets/js/myjavascript.mini.js")]
        [InlineData(typeof(TestApplicationC), "/server/assets/css/mycss.css")]
        [InlineData(typeof(TestApplicationC), "/server/assets/js/myjavascript.js")]
        [InlineData(typeof(TestApplicationC), "/server/assets/js/myjavascript.mini.js")]
        public void Uri(Type applicationType, string route)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var asset = componentHub.AssetManager.GetAssets(application)?
                .FirstOrDefault(x => x.Route.ToString() == route);

            // test execution
            Assert.Equal(route, asset?.Route.ToString());
        }

        /// <summary>
        /// Test the request of the asset.
        /// </summary>
        [Theory]
        [InlineData("http://localhost:8080/server/appa/assets/css/mycss.css", "css/mycss.css")]
        [InlineData("http://localhost:8080/server/appa/assets/css/my-css.css", "css/my-css.css")]
        [InlineData("http://localhost:8080/server/appa/assets/js/myjavascript.js", "js/myjavascript.js")]
        [InlineData("http://localhost:8080/server/appa/assets/js/myjavascript.mini.js", "js/myjavascript.mini.js")]
        [InlineData("http://localhost:8080/server/appb/assets/css/mycss.css", "css/mycss.css")]
        [InlineData("http://localhost:8080/server/appb/assets/js/myjavascript.js", "js/myjavascript.js")]
        [InlineData("http://localhost:8080/server/appb/assets/js/myjavascript.mini.js", "js/myjavascript.mini.js")]
        [InlineData("http://localhost:8080/server/assets/css/mycss.css", "css/mycss.css")]
        [InlineData("http://localhost:8080/server/assets/js/myjavascript.js", "js/myjavascript.js")]
        [InlineData("http://localhost:8080/server/assets/js/myjavascript.mini.js", "js/myjavascript.mini.js")]
        public void Request(string uri, string resource)
        {
            // preconditions
            var embeddedResource = UnitTestFixture.GetEmbeddedResource(resource);
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var context = UnitTestFixture.CreateHttpContextMock();
            var httpServerContext = UnitTestFixture.CreateHttpServerContextMock();
            componentHub.SitemapManager.Refresh();

            // test execution
            var searchResult = componentHub.SitemapManager.SearchResource(new System.Uri(uri), new SearchContext()
            {
                HttpServerContext = httpServerContext,
                Culture = httpServerContext.Culture,
                HttpContext = context
            });

            var response = componentHub
                .EndpointManager
                .HandleRequest(UnitTestFixture.CrerateRequestMock("", uri), searchResult.EndpointContext);

            Assert.Equal($"webexpress.webcore.test.{resource.Replace('/', '.')}", searchResult?.EndpointContext?.EndpointId.ToString());
            Assert.IsNotType<ResponseNotFound>(response);
            Assert.Equal(embeddedResource, Encoding.UTF8.GetString(response.Content as byte[]));
        }

        /// <summary>
        /// Tests whether the asset manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.AssetManager.GetType()));
        }

        /// <summary>
        /// Tests whether the asset context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var asset in componentHub.AssetManager.Assets)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(asset.GetType()), $"Asset context {asset.GetType().Name} does not implement IContext.");
            }
        }
    }
}
