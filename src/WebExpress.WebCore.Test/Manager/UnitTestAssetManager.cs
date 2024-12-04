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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(9, componentHub.AssetManager.Assets.Count());
        }

        /// <summary>
        /// Test the remove function of the asset manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
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
        [InlineData(typeof(TestApplicationA), "css.mycss.css")]
        [InlineData(typeof(TestApplicationA), "js.myjavascript.js")]
        [InlineData(typeof(TestApplicationA), "js.myjavascript.mini.js")]
        [InlineData(typeof(TestApplicationB), "css.mycss.css")]
        [InlineData(typeof(TestApplicationB), "js.myjavascript.js")]
        [InlineData(typeof(TestApplicationB), "js.myjavascript.mini.js")]
        [InlineData(typeof(TestApplicationC), "css.mycss.css")]
        [InlineData(typeof(TestApplicationC), "js.myjavascript.js")]
        [InlineData(typeof(TestApplicationC), "js.myjavascript.mini.js")]
        public void Id(Type applicationType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var asset = componentHub.AssetManager.GetAssets(application)?.FirstOrDefault(x => x.EndpointId == id);

            // test execution
            Assert.Equal(id, asset?.EndpointId);
        }

        /// <summary>
        /// Test the uri property of the asset.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/server/appa/assets/css.mycss.css")]
        [InlineData(typeof(TestApplicationA), "/server/appa/assets/js.myjavascript.js")]
        [InlineData(typeof(TestApplicationA), "/server/appa/assets/js.myjavascript.mini.js")]
        [InlineData(typeof(TestApplicationB), "/server/appb/assets/css.mycss.css")]
        [InlineData(typeof(TestApplicationB), "/server/appb/assets/js.myjavascript.js")]
        [InlineData(typeof(TestApplicationB), "/server/appb/assets/js.myjavascript.mini.js")]
        [InlineData(typeof(TestApplicationC), "/server/assets/css.mycss.css")]
        [InlineData(typeof(TestApplicationC), "/server/assets/js.myjavascript.js")]
        [InlineData(typeof(TestApplicationC), "/server/assets/js.myjavascript.mini.js")]
        public void Uri(Type applicationType, string uri)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var asset = componentHub.AssetManager.GetAssets(application)?.FirstOrDefault(x => x.EndpointId == Path.GetFileName(uri));

            // test execution
            Assert.Equal(uri, asset?.Uri);
        }

        /// <summary>
        /// Test the request of the asset.
        /// </summary>
        [Theory]
        [InlineData("http://localhost:8080/server/appa/assets/css/mycss.css", "webexpress.webcore.asset", "css.mycss.css")]
        [InlineData("http://localhost:8080/server/appa/assets/js/myjavascript.js", "webexpress.webcore.asset", "js.myjavascript.js")]
        [InlineData("http://localhost:8080/server/appa/assets/js/myjavascript.mini.js", "webexpress.webcore.asset", "js.myjavascript.mini.js")]
        [InlineData("http://localhost:8080/server/appa/assets/css.mycss.css", "webexpress.webcore.asset", "css.mycss.css")]
        [InlineData("http://localhost:8080/server/appa/assets/js.myjavascript.js", "webexpress.webcore.asset", "js.myjavascript.js")]
        [InlineData("http://localhost:8080/server/appa/assets/js.myjavascript.mini.js", "webexpress.webcore.asset", "js.myjavascript.mini.js")]
        [InlineData("http://localhost:8080/server/appb/assets/css/mycss.css", "webexpress.webcore.asset", "css.mycss.css")]
        [InlineData("http://localhost:8080/server/appb/assets/js/myjavascript.js", "webexpress.webcore.asset", "js.myjavascript.js")]
        [InlineData("http://localhost:8080/server/appb/assets/js/myjavascript.mini.js", "webexpress.webcore.asset", "js.myjavascript.mini.js")]
        [InlineData("http://localhost:8080/server/appb/assets/css.mycss.css", "webexpress.webcore.asset", "css.mycss.css")]
        [InlineData("http://localhost:8080/server/appb/assets/js.myjavascript.js", "webexpress.webcore.asset", "js.myjavascript.js")]
        [InlineData("http://localhost:8080/server/appb/assets/js.myjavascript.mini.js", "webexpress.webcore.asset", "js.myjavascript.mini.js")]
        [InlineData("http://localhost:8080/server/assets/css/mycss.css", "webexpress.webcore.asset", "css.mycss.css")]
        [InlineData("http://localhost:8080/server/assets/js/myjavascript.js", "webexpress.webcore.asset", "js.myjavascript.js")]
        [InlineData("http://localhost:8080/server/assets/js/myjavascript.mini.js", "webexpress.webcore.asset", "js.myjavascript.mini.js")]
        [InlineData("http://localhost:8080/server/assets/css.mycss.css", "webexpress.webcore.asset", "css.mycss.css")]
        [InlineData("http://localhost:8080/server/assets/js.myjavascript.js", "webexpress.webcore.asset", "js.myjavascript.js")]
        [InlineData("http://localhost:8080/server/assets/js.myjavascript.mini.js", "webexpress.webcore.asset", "js.myjavascript.mini.js")]
        public void Request(string uri, string id, string resource)
        {
            // preconditions
            var embeddedResource = UnitTestControlFixture.GetEmbeddedResource(resource);
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var context = UnitTestControlFixture.CreateHttpContextMock();
            var httpServerContext = UnitTestControlFixture.CreateHttpServerContextMock();
            componentHub.SitemapManager.Refresh();

            // test execution
            var searchResult = componentHub.SitemapManager.SearchResource(new System.Uri(uri), new SearchContext()
            {
                HttpServerContext = httpServerContext,
                Culture = httpServerContext.Culture,
                HttpContext = context
            });

            var response = componentHub.EndpointManager.HandleRequest(UnitTestControlFixture.CrerateRequestMock("", uri), searchResult.EndpointContext);

            Assert.Equal(id, searchResult?.EndpointContext?.EndpointId);
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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var asset in componentHub.AssetManager.Assets)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(asset.GetType()), $"Asset context {asset.GetType().Name} does not implement IContext.");
            }
        }
    }
}
