using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.Test.Uri
{
    /// <summary>
    /// Tests the extended path property.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestUriRelativeExtendedPath
    {
        private readonly UriResource Uri = new UriResource("http://user@example.com:80");

        [Fact]
        public void ExtendedPath_0()
        {
            var segments = new List<IUriPathSegment>
            {
                new UriPathSegmentRoot(),
                new UriPathSegmentConstant("a"),
                new UriPathSegmentConstant("b"),
                new UriPathSegmentConstant("c"),
                new UriPathSegmentConstant("x"),
                new UriPathSegmentConstant("y")
            };

            var resourceUri = new UriResource(Uri, segments);
            resourceUri.ServerRoot = new UriResource("http://user@example.com:80");
            resourceUri.ApplicationRoot = new UriResource("http://user@example.com:80");
            resourceUri.ModuleRoot = new UriResource("http://user@example.com:80");
            resourceUri.ResourceRoot = new UriResource("http://user@example.com:80/a/b/c");

            Assert.True
            (
                resourceUri.ToString() == "http://user@example.com/a/b/c/x/y" &&
                resourceUri.ExtendedPath.ToString() == "/x/y" &&
                resourceUri.Scheme == UriScheme.Http &&
                resourceUri.Authority.User == "user" &&
                resourceUri.Authority.Host == "example.com" &&
                resourceUri.Authority.Port == 80 &&
                resourceUri.ServerRoot != null &&
                resourceUri.IsRelative == false
            );
        }
    }
}
