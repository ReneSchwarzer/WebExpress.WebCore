using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.Test.Web.Home;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the sitemap manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestSitemapManager
    {
        /// <summary>
        /// Test the refresh function of the sitemap manager.
        /// </summary>
        [Fact]
        public void Refresh()
        {
            // preconditions
            var componentManager = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            componentManager.SitemapManager.Refresh();

            Assert.Equal(58, componentManager.SitemapManager.SiteMap.Count());
        }

        /// <summary>
        /// Test the search resource function of the sitemap.
        /// </summary>
        [Theory]
        [InlineData("http://localhost:8080/server/appa/resa", "webexpress.webcore.test.testresourcea")]
        [InlineData("http://localhost:8080/server/appa/resa/resb", "webexpress.webcore.test.testresourceb")]
        [InlineData("http://localhost:8080/server/appa/resc", "webexpress.webcore.test.testresourcec")]
        [InlineData("http://localhost:8080/server/appa/resd", "webexpress.webcore.test.testresourced")]
        [InlineData("http://localhost:8080/server/appb/resa", "webexpress.webcore.test.testresourcea")]
        [InlineData("http://localhost:8080/server/appb/resa/resb", "webexpress.webcore.test.testresourceb")]
        [InlineData("http://localhost:8080/server/appb/resc", "webexpress.webcore.test.testresourcec")]
        [InlineData("http://localhost:8080/server/appb/resd", "webexpress.webcore.test.testresourced")]
        [InlineData("http://localhost:8080/server/resa", "webexpress.webcore.test.testresourcea")]
        [InlineData("http://localhost:8080/server/resa/resb", "webexpress.webcore.test.testresourceb")]
        [InlineData("http://localhost:8080/server/resc", "webexpress.webcore.test.testresourcec")]
        [InlineData("http://localhost:8080/server/resd", "webexpress.webcore.test.testresourced")]
        [InlineData("http://localhost:8080/server/appa/pagea", "webexpress.webcore.test.testpagea")]
        [InlineData("http://localhost:8080/server/appa/resa/B/testpageb", "webexpress.webcore.test.testpageb")]
        [InlineData("http://localhost:8080/server/appa/", "webexpress.webcore.test.testpagec")]
        [InlineData("http://localhost:8080/server/appb/pagea", "webexpress.webcore.test.testpagea")]
        [InlineData("http://localhost:8080/server/appb/resa/b/testpageb", "webexpress.webcore.test.testpageb")]
        [InlineData("http://localhost:8080/server/appb/", "webexpress.webcore.test.testpagec")]
        [InlineData("http://localhost:8080/server/pagea", "webexpress.webcore.test.testpagea")]
        [InlineData("http://localhost:8080/server/resa/b/TestPageB", "webexpress.webcore.test.testpageb")]
        [InlineData("http://localhost:8080/server", "webexpress.webcore.test.testpagec")]
        [InlineData("http://localhost:8080/server/", "webexpress.webcore.test.testpagec")]
        [InlineData("http://localhost:8080/server/appa/1/apia", "webexpress.webcore.test.testrestapia")]
        [InlineData("http://localhost:8080/server/appa/1/apia/2/apib", "webexpress.webcore.test.testrestapib")]
        [InlineData("http://localhost:8080/server/appa/1/apia/2/apib/3/apic", "webexpress.webcore.test.testrestapic")]
        [InlineData("http://localhost:8080/server/appa/assets/css/mycss.css", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/server/appa/assets/js/myjavascript.js", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/server/appa/assets/js/myjavascript.mini.js", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/server/appa/assets/css.mycss.css", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/server/appa/assets/js.myjavascript.js", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/server/appa/assets/js.myjavascript.mini.js", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/uri/does/not/exist", null)]

        public void SearchResource(string uri, string id)
        {
            // preconditions
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

            componentHub.EndpointManager.HandleRequest(UnitTestFixture.CrerateRequestMock(), searchResult.EndpointContext);

            Assert.Equal(id, searchResult?.EndpointContext?.EndpointId.ToString());
        }

        /// <summary>
        /// Test the get uri function of the sitemap.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestResourceA), "/server/appa/resa")]
        [InlineData(typeof(TestResourceB), "/server/appa/resa/resb")]
        [InlineData(typeof(TestResourceC), "/server/appa/resc")]
        [InlineData(typeof(TestResourceD), "/server/appa/resd")]
        [InlineData(typeof(TestPageA), "/server/appa/pagea")]
        [InlineData(typeof(TestPageB), "/server/appa/resa/b/testpageb")]
        [InlineData(typeof(TestPageC), "/server")]
        [InlineData(typeof(TestPageH), "/server/appa/web/home/testpageh")]
        public void GetUri(Type resourceType, string expected)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            componentHub.SitemapManager.Refresh();

            // test execution
            var uri = componentHub.SitemapManager.GetUri(resourceType);

            Assert.Equal(expected, uri?.ToString());
        }

        /// <summary>
        /// Test the get endpoint function of the sitemap.
        /// </summary>
        [Theory]
        [InlineData("http://localhost:8080/server/appa/resa", "webexpress.webcore.test.testresourcea")]
        [InlineData("http://localhost:8080/server/appa/resa/resb", "webexpress.webcore.test.testresourceb")]
        [InlineData("http://localhost:8080/server/appa/resc", "webexpress.webcore.test.testresourcec")]
        [InlineData("http://localhost:8080/server/appa/resd", "webexpress.webcore.test.testresourced")]
        [InlineData("http://localhost:8080/server/appb/resa", "webexpress.webcore.test.testresourcea")]
        [InlineData("http://localhost:8080/server/appb/resa/resb", "webexpress.webcore.test.testresourceb")]
        [InlineData("http://localhost:8080/server/appb/resc", "webexpress.webcore.test.testresourcec")]
        [InlineData("http://localhost:8080/server/appb/resd", "webexpress.webcore.test.testresourced")]
        [InlineData("http://localhost:8080/server/resa", "webexpress.webcore.test.testresourcea")]
        [InlineData("http://localhost:8080/server/resa/resb", "webexpress.webcore.test.testresourceb")]
        [InlineData("http://localhost:8080/server/resc", "webexpress.webcore.test.testresourcec")]
        [InlineData("http://localhost:8080/server/resd", "webexpress.webcore.test.testresourced")]
        [InlineData("http://localhost:8080/server/appa/pagea", "webexpress.webcore.test.testpagea")]
        [InlineData("http://localhost:8080/server/appa/resa/B/testpageb", "webexpress.webcore.test.testpageb")]
        [InlineData("http://localhost:8080/server/appa/", "webexpress.webcore.test.testpagec")]
        [InlineData("http://localhost:8080/server/appb/pagea", "webexpress.webcore.test.testpagea")]
        [InlineData("http://localhost:8080/server/appb/resa/b/TestPageB", "webexpress.webcore.test.testpageb")]
        [InlineData("http://localhost:8080/server/appb/", "webexpress.webcore.test.testpagec")]
        [InlineData("http://localhost:8080/server/pagea", "webexpress.webcore.test.testpagea")]
        [InlineData("http://localhost:8080/server/resa/b/testpageb", "webexpress.webcore.test.testpageb")]
        [InlineData("http://localhost:8080/server", "webexpress.webcore.test.testpagec")]
        [InlineData("http://localhost:8080/server/", "webexpress.webcore.test.testpagec")]
        [InlineData("http://localhost:8080/server/appa/1/apia", "webexpress.webcore.test.testrestapia")]
        [InlineData("http://localhost:8080/server/appa/1/apia/2/apib", "webexpress.webcore.test.testrestapib")]
        [InlineData("http://localhost:8080/server/appa/1/apia/2/apib/3/apic", "webexpress.webcore.test.testrestapic")]
        [InlineData("http://localhost:8080/server/appa/assets/css/mycss.css", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/server/appa/assets/js/myjavascript.js", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/server/appa/assets/js/myjavascript.mini.js", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/server/appa/assets/css.mycss.css", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/server/appa/assets/js.myjavascript.js", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/server/appa/assets/js.myjavascript.mini.js", "webexpress.webcore.asset")]
        [InlineData("http://localhost:8080/uri/does/not/exist", null)]
        public void GetEndpoint(string uri, string expected)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            componentHub.SitemapManager.Refresh();

            // test execution
            var endpoint = componentHub.SitemapManager.GetEndpoint(new UriResource(uri));

            Assert.Equal(expected, endpoint?.EndpointId?.ToString());
        }

        /// <summary>
        /// Tests whether the sitemap manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.SitemapManager.GetType()));
        }
    }
}
