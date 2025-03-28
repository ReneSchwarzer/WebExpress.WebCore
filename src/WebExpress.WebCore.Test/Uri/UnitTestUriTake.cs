using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.Test.Uri
{
    /// <summary>
    /// Tests the take method.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestUriTake
    {
        /// <summary>
        /// Test the take method.
        /// </summary>
        [Theory]
        [InlineData("/a/b/c", 0, "/", 0)]
        [InlineData("/a/b/c", 1, "/", 1)]
        [InlineData("/a/b/c", 2, "/a", 2)]
        [InlineData("/a/b/c", 3, "/a/b", 3)]
        [InlineData("/a/b/c", 4, "/a/b/c", 4)]
        [InlineData("/a/b/c", 5, "/a/b/c", 4)]
        [InlineData("/a/b/c", -1, "/a/b", 3)]
        [InlineData("/a/b/c", -2, "/a", 2)]
        [InlineData("/a/b/c", -3, "/", 1)]
        [InlineData("/a/b/c", -4, null, 0)]
        [InlineData("/a/b/c", -5, null, 0)]
        public void Take(string path, int takeCount, string expected, int count)
        {
            // preconditions
            var uri = new UriEndpoint(path);

            // test execution
            var take = uri.Take(takeCount);

            Assert.Equal(expected, take?.ToString());
            Assert.Equal(count, take?.PathSegments.Count() ?? 0);
        }
    }
}
