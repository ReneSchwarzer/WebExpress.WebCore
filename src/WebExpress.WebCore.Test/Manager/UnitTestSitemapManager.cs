using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebSitemap;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the resource manager.
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
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();

            // test execution
            componentManager.SitemapManager.Refresh();

            Assert.Equal(13, componentManager.SitemapManager.SiteMap.Count());
        }

        /// <summary>
        /// Test the SearchResource function of the sitemap.
        /// </summary>
        [Theory]
        [InlineData("http://localhost:8080/aca/mca/a1x", "webexpress.webcore.test.testresourcea1x")]
        [InlineData("http://localhost:8080/aca/mca/a1x/a1y", "webexpress.webcore.test.testresourcea1y")]
        [InlineData("http://localhost:8080/aca/a2x", "webexpress.webcore.test.testresourcea2x")]
        [InlineData("http://localhost:8080/aca/mcab/ab1x", "webexpress.webcore.test.testresourceab1x")]
        [InlineData("http://localhost:8080/acb/mcab/ab1x", "webexpress.webcore.test.testresourceab1x")]
        [InlineData("http://localhost:8080/uri/does/not/exist", null)]

        public void SearchResource(string uri, string id)
        {
            // preconditions
            using var componentMoinitor = new ResourceMonitor();
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var context = UnitTestControlFixture.CreateHttpContext();
            componentManager.SitemapManager.Refresh();
            var map = componentManager.SitemapManager.ToString();

            // test execution
            var searchResult = componentManager.SitemapManager.SearchResource(new System.Uri(uri), new SearchContext()
            {
                HttpServerContext = componentManager.HttpServerContext,
                Culture = componentManager.HttpServerContext.Culture,
                HttpContext = context
            });

            Assert.Equal(id, searchResult.ResourceId);
            //Assert.Single(componentMoinitor.Contains(resourceType));
        }

        /// <summary>
        /// Test the GetUri function of the sitemap.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestResourceA1X), "/aca/mca/a1x")]
        [InlineData(typeof(TestResourceA1Y), "/aca/mca/a1x/a1y")]
        [InlineData(typeof(TestResourceA2X), "/aca/a2x")]
        [InlineData(typeof(TestResourceAB1X), "/aca/mcab/ab1x")]

        public void GetUri(Type resourceType, string expected)
        {
            // preconditions
            using var componentMoinitor = new ResourceMonitor();
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var context = UnitTestControlFixture.CreateHttpContext();
            componentManager.SitemapManager.Refresh();
            //var ressource = componentManager.ResourceManager.GetResorces(resourceType).FirstOrDefault();

            // test execution
            var uri = componentManager.SitemapManager.GetUri(resourceType);

            Assert.Equal(expected, uri?.ToString());
            Assert.False(componentMoinitor.Contains(resourceType));
        }
    }
}
