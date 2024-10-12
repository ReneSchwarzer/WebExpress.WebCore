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

            Assert.Equal(22, componentManager.SitemapManager.SiteMap.Count());
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
        [InlineData("http://localhost:8080/aca/mca/pa1x", "webexpress.webcore.test.testpagea1x")]
        [InlineData("http://localhost:8080/aca/mca/1/ra1x", "webexpress.webcore.test.testrestapia1x")]
        [InlineData("http://localhost:8080/aca/mca/1/ra1x/2/ra1y", "webexpress.webcore.test.testrestapia1y")]
        [InlineData("http://localhost:8080/aca/mca/1/ra1x/2/ra1y/3/ra1z", "webexpress.webcore.test.testrestapia1z")]
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

            searchResult.Process(UnitTestControlFixture.CrerateRequestMock());

            Assert.Equal(id, searchResult.EndpointId);
        }

        /// <summary>
        /// Test the GetUri function of the sitemap.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestResourceA1X), "/aca/mca/a1x")]
        [InlineData(typeof(TestResourceA1Y), "/aca/mca/a1x/a1y")]
        [InlineData(typeof(TestResourceA2X), "/aca/a2x")]
        [InlineData(typeof(TestResourceAB1X), "/aca/mcab/ab1x")]
        [InlineData(typeof(TestPageA1X), "/aca/mca/pa1x")]
        [InlineData(typeof(TestPageA1Y), "/aca/mca/pa1y")]
        [InlineData(typeof(TestPageA1Z), "/aca/mca/pa1z")]

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
