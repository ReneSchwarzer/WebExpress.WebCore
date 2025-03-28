using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.Test.Uri
{
    /// <summary>
    /// Tests the concat method.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestUriConcat
    {
        /// <summary>
        /// Test the concat method.
        /// </summary>
        [Theory]
        [InlineData("/a/b/c", null, "/a/b/c", 4)]
        [InlineData("/a/b/c", "", "/a/b/c", 4)]
        [InlineData("/a/b/c", "d", "/a/b/c/d", 5)]
        [InlineData("/a/b/c", "/d/e/f", "/a/b/c/d/e/f", 7)]
        public void Concat(string path, string segment, string expected, int count)
        {
            // preconditions
            var uri = new UriEndpoint(path);

            // test execution
            var concat = uri.Concat(segment);

            Assert.Equal(expected, concat.ToString());
            Assert.Equal(count, concat.PathSegments.Count());
        }
    }
}
