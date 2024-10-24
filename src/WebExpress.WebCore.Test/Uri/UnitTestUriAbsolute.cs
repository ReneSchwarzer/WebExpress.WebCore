using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.Test.Uri
{
    /// <summary>
    /// Tests an absolute Uri.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestUriAbsolute
    {
        [Fact]
        public void Test_0()
        {
            var str = "http://user@example.com:8080/abc#a?b=1&c=2";
            var uri = new UriResource(str);

            Assert.Equal(uri.ToString(), str);
            Assert.Equal(UriScheme.Http, uri.Scheme);
            Assert.Equal("user", uri.Authority.User);
            Assert.Equal("example.com", uri.Authority.Host);
            Assert.Equal(8080, uri.Authority.Port);
            Assert.Equal("a", uri.Fragment);
            Assert.Equal("b", uri.Query.FirstOrDefault()?.Key);
            Assert.Equal("1", uri.Query.FirstOrDefault()?.Value);
            Assert.Equal("c", uri.Query.LastOrDefault()?.Key);
            Assert.Equal("2", uri.Query.LastOrDefault()?.Value);
            Assert.False(uri.IsRelative);
        }

        [Fact]
        public void Test_1()
        {
            var str = "http://vila/assets/img/vila.svg";
            var uri = new UriResource(str);

            Assert.True
            (
                uri.ToString() == str &&
                uri.Scheme == UriScheme.Http &&
                uri.Authority.User == null &&
                uri.Authority.Host == "vila" &&
                uri.Authority.Port == null &&
                uri.Fragment == null &&
                uri.Query.Any() == false &&
                uri.IsRelative == false
            );
        }

        [Fact]
        public void Test_2()
        {
            var str = "http://localhost";
            var uri = new UriResource(str);

            Assert.True
            (
                uri.ToString() == str + "/" &&
                uri.Scheme == UriScheme.Http &&
                uri.Authority.User == null &&
                uri.Authority.Host == "localhost" &&
                uri.Authority.Port == null &&
                uri.Fragment == null &&
                uri.Query.Any() == false &&
                uri.IsRelative == false
            );
        }

        [Fact]
        public void Test_3()
        {
            var str = "http://user@example.com:80/abc#a?b=1&c=2";
            var uri = new UriResource(str);

            Assert.True
            (
                uri.ToString() == "http://user@example.com/abc#a?b=1&c=2" &&
                uri.Scheme == UriScheme.Http &&
                uri.Authority.User == "user" &&
                uri.Authority.Host == "example.com" &&
                uri.Authority.Port == 80 &&
                uri.Fragment == "a" &&
                uri.Query.FirstOrDefault()?.Key == "b" &&
                uri.Query.FirstOrDefault()?.Value == "1" &&
                uri.Query.LastOrDefault()?.Key == "c" &&
                uri.Query.LastOrDefault()?.Value == "2" &&
                uri.IsRelative == false
            );
        }

        [Fact]
        public void Test_4()
        {
            var str = "http://user@example.com:80/abc#a?b=1&c=2";
            var uri = new UriResource(str);
            var segments = new List<IUriPathSegment>
            {
                new UriPathSegmentRoot(),
                new UriPathSegmentConstant("a"),
                new UriPathSegmentConstant("b"),
                new UriPathSegmentConstant("c")
            };

            var extendetSegments = new List<IUriPathSegment>
            {
                new UriPathSegmentRoot(),
                new UriPathSegmentConstant("x"),
                new UriPathSegmentConstant("y")
            };

            var resourceUri = new UriResource(uri, segments);
            resourceUri = new UriResource(resourceUri, resourceUri.PathSegments, extendetSegments);
            resourceUri.ServerRoot = new UriResource("http://user@example.com:80");
            resourceUri.ApplicationRoot = new UriResource("http://user@example.com:80");
            resourceUri.EndpointRoot = new UriResource("http://user@example.com:80/abc");

            Assert.True
            (
                resourceUri.ToString() == "http://user@example.com/a/b/c/x/y#a?b=1&c=2" &&
                resourceUri.Scheme == UriScheme.Http &&
                resourceUri.Authority.User == "user" &&
                resourceUri.Authority.Host == "example.com" &&
                resourceUri.Authority.Port == 80 &&
                resourceUri.Fragment == "a" &&
                resourceUri.Query.FirstOrDefault()?.Key == "b" &&
                resourceUri.Query.FirstOrDefault()?.Value == "1" &&
                resourceUri.Query.LastOrDefault()?.Key == "c" &&
                resourceUri.Query.LastOrDefault()?.Value == "2" &&
                resourceUri.ServerRoot != null &&
                resourceUri.IsRelative == false
            );
        }
    }
}
