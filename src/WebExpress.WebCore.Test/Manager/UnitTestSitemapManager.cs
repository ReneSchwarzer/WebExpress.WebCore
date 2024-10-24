using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebSitemap;

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
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            componentManager.SitemapManager.Refresh();

            Assert.Equal(42, componentManager.SitemapManager.SiteMap.Count());
        }

        /// <summary>
        /// Test the SearchResource function of the sitemap.
        /// </summary>
        [Theory]
        [InlineData("http://localhost:8080/appa/resa", "webexpress.webcore.test.testresourcea")]
        [InlineData("http://localhost:8080/appa/resa/resb", "webexpress.webcore.test.testresourceb")]
        [InlineData("http://localhost:8080/appa/resc", "webexpress.webcore.test.testresourcec")]
        [InlineData("http://localhost:8080/appa/resd", "webexpress.webcore.test.testresourced")]
        [InlineData("http://localhost:8080/appb/resa", "webexpress.webcore.test.testresourcea")]
        [InlineData("http://localhost:8080/appb/resa/resb", "webexpress.webcore.test.testresourceb")]
        [InlineData("http://localhost:8080/appb/resc", "webexpress.webcore.test.testresourcec")]
        [InlineData("http://localhost:8080/appb/resd", "webexpress.webcore.test.testresourced")]
        [InlineData("http://localhost:8080/resa", "webexpress.webcore.test.testresourcea")]
        [InlineData("http://localhost:8080/resa/resb", "webexpress.webcore.test.testresourceb")]
        [InlineData("http://localhost:8080/resc", "webexpress.webcore.test.testresourcec")]
        [InlineData("http://localhost:8080/resd", "webexpress.webcore.test.testresourced")]
        [InlineData("http://localhost:8080/appa/pagea", "webexpress.webcore.test.testpagea")]
        [InlineData("http://localhost:8080/appa/resa/pageb", "webexpress.webcore.test.testpageb")]
        [InlineData("http://localhost:8080/appa/pagec", "webexpress.webcore.test.testpagec")]
        [InlineData("http://localhost:8080/appb/pagea", "webexpress.webcore.test.testpagea")]
        [InlineData("http://localhost:8080/appb/resa/pageb", "webexpress.webcore.test.testpageb")]
        [InlineData("http://localhost:8080/appb/pagec", "webexpress.webcore.test.testpagec")]
        [InlineData("http://localhost:8080/pagea", "webexpress.webcore.test.testpagea")]
        [InlineData("http://localhost:8080/resa/pageb", "webexpress.webcore.test.testpageb")]
        [InlineData("http://localhost:8080/pagec", "webexpress.webcore.test.testpagec")]
        [InlineData("http://localhost:8080/appa/1/apia", "webexpress.webcore.test.testrestapia")]
        [InlineData("http://localhost:8080/appa/1/apia/2/apib", "webexpress.webcore.test.testrestapib")]
        [InlineData("http://localhost:8080/appa/1/apia/2/apib/3/apic", "webexpress.webcore.test.testrestapic")]
        [InlineData("http://localhost:8080/uri/does/not/exist", null)]

        public void SearchResource(string uri, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var context = UnitTestControlFixture.CreateHttpContextMock();
            componentHub.SitemapManager.Refresh();

            // test execution
            var searchResult = componentHub.SitemapManager.SearchResource(new System.Uri(uri), new SearchContext()
            {
                HttpServerContext = componentHub.HttpServerContext,
                Culture = componentHub.HttpServerContext.Culture,
                HttpContext = context
            });

            componentHub.EndpointManager.HandleRequest(UnitTestControlFixture.CrerateRequestMock(), searchResult.EndpointContext);

            Assert.Equal(id, searchResult?.EndpointContext?.EndpointId);
        }

        /// <summary>
        /// Test the GetUri function of the sitemap.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestResourceA), "/appa/resa")]
        [InlineData(typeof(TestResourceB), "/appa/resa/resb")]
        [InlineData(typeof(TestResourceC), "/appa/resc")]
        [InlineData(typeof(TestResourceD), "/appa/resd")]
        [InlineData(typeof(TestPageA), "/appa/pagea")]
        [InlineData(typeof(TestPageB), "/appa/resa/pageb")]
        [InlineData(typeof(TestPageC), "/appa/pagec")]

        public void GetUri(Type resourceType, string expected)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            componentHub.SitemapManager.Refresh();

            // test execution
            var uri = componentHub.SitemapManager.GetUri(resourceType);

            Assert.Equal(expected, uri?.ToString());
        }

        /// <summary>
        /// Tests whether the sitemap manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.SitemapManager.GetType()));
        }
    }

}
