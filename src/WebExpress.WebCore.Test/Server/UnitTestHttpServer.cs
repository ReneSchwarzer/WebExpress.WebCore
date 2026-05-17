using WebExpress.WebCore.Test.Fixture;

namespace WebExpress.WebCore.Test.Server
{
    /// <summary>
    /// Test the HTTP server.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestHttpServer
    {
        /// <summary>
        /// Asynchronously processes an HTTP request using the specified HTTP context.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessRequestAsync()
        {
            // arrange
            var content = "GET /server/appa/api/1/testrestapia HTTP/1.1\n" +
            "Authorization: Bearer abc123\n" +
            "X-Test: 123\n" +
            "X-Mode: UnitTest\n" +
            "\n";
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var httpServerContext = UnitTestFixture.CreateHttpServerContextMock();
            var httpContext = UnitTestFixture.CreateHttpContextMock(content);
            var server = new HttpServer(httpServerContext);
            componentHub.SitemapManager.Refresh();

            // act
            await server.ProcessRequestAsync(httpContext);

            // validation
            Assert.NotNull(server);
        }
    }
}
