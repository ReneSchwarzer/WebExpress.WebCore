using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.Test.Uri
{
    /// <summary>
    /// Tests the skip method.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestUriSkip
    {
        /// <summary>
        /// Test the skip method.
        /// </summary>
        [Theory]
        [InlineData("/a/b/c", 0, "/a/b/c", 4)]
        [InlineData("/a/b/c", 1, "/a/b/c", 3)]
        [InlineData("/a/b/c", 2, "/b/c", 2)]
        [InlineData("/a/b/c", 3, "/c", 1)]
        [InlineData("/a/b/c", 4, null, 0)]
        [InlineData("/a/b/c", 5, null, 0)]
        public void Skip(string path, int skipCount, string expected, int count)
        {
            // preconditions
            var uri = new UriEndpoint(path);

            // test execution
            var skip = uri.Skip(skipCount);

            Assert.Equal(expected, skip?.ToString());
            Assert.Equal(count, skip?.PathSegments.Count() ?? 0);
        }
    }
}
