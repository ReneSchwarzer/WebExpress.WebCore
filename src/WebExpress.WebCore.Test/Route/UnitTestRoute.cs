using WebExpress.WebCore.WebEndpoint;

namespace WebExpress.WebCore.Test.Route
{
    /// <summary>
    /// Tests the route.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestRoute
    {
        /// <summary>
        /// Test the concat method.
        /// </summary>
        [Theory]
        [InlineData("/a/b/c", null, "/a/b/c", 4)]
        [InlineData("/a/b/c", "", "/a/b/c", 4)]
        [InlineData("/a/b/c", "d", "/a/b/c/d", 5)]
        [InlineData("/a/b/c", "/d/e/f", "/a/b/c/d/e/f", 7)]
        public void Concat(string baseRoute, string segment, string expected, int count)
        {
            // preconditions
            var uri = new RouteEndpoint(baseRoute);

            // test execution
            var concat = uri.Concat(segment);

            Assert.Equal(expected, concat.ToString());
            Assert.Equal(count, concat.PathSegments.Count());
        }

        /// <summary>
        /// Test the combine method.
        /// </summary>
        [Theory]
        [InlineData("/a/b/c", null, "/a/b/c")]
        [InlineData("/a/b/c", "", "/a/b/c")]
        [InlineData("/a/b/c", "d", "/a/b/c/d")]
        [InlineData("/a/b/c", "/d/e/f", "/a/b/c/d/e/f")]
        public void CombinePath(string baseRoute, string pathB, string expected)
        {
            // test execution
            var combine = RouteEndpoint.Combine([new RouteEndpoint(baseRoute), new RouteEndpoint(pathB)]);

            Assert.Equal(expected, combine.ToString());
        }

        /// <summary>
        /// Test the combine method.
        /// </summary>
        [Theory]
        [InlineData("/a/b/c", null, "/a/b/c")]
        [InlineData("/a/b/c", "", "/a/b/c")]
        [InlineData("/a/b/c", "d", "/a/b/c/d")]
        [InlineData("/a/b/c", "/d/e/f", "/a/b/c/d/e/f")]
        public void CombineRoute(string baseRoute, string pathB, string expected)
        {
            // test execution
            var combine = RouteEndpoint.Combine(new RouteEndpoint(baseRoute), [pathB]);

            Assert.Equal(expected, combine.ToString());
        }

        /// <summary>
        /// Test the combine method.
        /// </summary>
        [Theory]
        [InlineData("/a/b/c", null, "/a/b/c")]
        [InlineData("/a/b/c", "", "/a/b/c")]
        [InlineData("/a/b/c", "d", "/a/b/c/d")]
        [InlineData("/a/b/c", "/d/e/f", "/a/b/c/d/e/f")]
        public void CombineSegment(string baseRoute, string segment, string expected)
        {
            // test execution
            var combine = RouteEndpoint.Combine(new RouteEndpoint(baseRoute), segment);

            Assert.Equal(expected, combine.ToString());
        }
    }
}
