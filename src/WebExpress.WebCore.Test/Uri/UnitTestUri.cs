using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.Test.Uri
{
    /// <summary>
    /// Tests an uri.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestUri
    {
        /// <summary>
        /// Test the constructor with absolute URIs.
        /// </summary>
        [Theory]
        [InlineData(UriScheme.Http, "www.example.com", null, null, null, "a=1&b=2", "fragment", "http://www.example.com/?a=1&b=2#fragment")]
        [InlineData(UriScheme.Http, "www.example.com", null, null, "", "a=1&b=2", "fragment", "http://www.example.com/?a=1&b=2#fragment")]
        [InlineData(UriScheme.Http, "www.example.com", null, null, "/abc", "a=1&b=2", "fragment", "http://www.example.com/abc?a=1&b=2#fragment")]
        [InlineData(UriScheme.Http, "example.com", "user", "8080", "/abc", "a=1&b=2", "fragment", "http://user@example.com:8080/abc?a=1&b=2#fragment")]
        [InlineData(UriScheme.Http, "example", null, null, "/assets/img/example.svg", null, null, "http://example/assets/img/example.svg")]
        [InlineData(UriScheme.Http, "localhost", null, null, null, null, null, "http://localhost/")]
        [InlineData(UriScheme.Http, "localhost", null, null, "/", null, null, "http://localhost/")]
        [InlineData(UriScheme.Http, "example.com", "user", "80", "/abc", "a=1&b=2", "fragment", "http://user@example.com/abc?a=1&b=2#fragment")]
        public void UriAbsolute(UriScheme scheme, string authority, string user, string port, string path, string query, string fragment, string expected)
        {
            // preconditions
            var uriUser = user != null ? user + "@" : "";
            var uriPort = port != null ? ":" + port : null;
            var uriQuery = query != null ? "?" + query : "";
            var uriFragment = fragment != null ? "#" + fragment : null;

            // test execution
            var uri = new UriEndpoint($"{scheme}://{uriUser}{authority}{uriPort}{path}{uriQuery}{uriFragment}");

            Assert.Equal(expected, uri.ToString());
            Assert.Equal(scheme, uri.Scheme);
            Assert.Equal(authority, uri.Authority.Host);
            Assert.Equal(path, !string.IsNullOrWhiteSpace(path)
                ? "/" + string.Join("/", uri.PathSegments.Skip(1))
                : path);
            Assert.Equal(query, uri.Query.Any() ? string.Join("&", uri.Query) : null);
            Assert.Equal(fragment, uri.Fragment);
        }

        /// <summary>
        /// Test the constructor with relative URIs.
        /// </summary>
        [Theory]
        [InlineData(null, null, null, "/")]
        [InlineData(null, "a=1&b=2", null, "/?a=1&b=2")]
        [InlineData(null, null, "fragment", "/#fragment")]
        [InlineData(null, "a=1&b=2", "fragment", "/?a=1&b=2#fragment")]
        [InlineData("", null, null, "/")]
        [InlineData("", "a=1&b=2", null, "/?a=1&b=2")]
        [InlineData("", null, "fragment", "/#fragment")]
        [InlineData("", "a=1&b=2", "fragment", "/?a=1&b=2#fragment")]
        [InlineData("/", null, null, "/")]
        [InlineData("/", "a=1&b=2", null, "/?a=1&b=2")]
        [InlineData("/abc", "a=1&b=2", "fragment", "/abc?a=1&b=2#fragment")]
        [InlineData("/assets/img/example.svg", null, null, "/assets/img/example.svg")]
        public void UriRelative(string path, string query, string fragment, string expected)
        {
            // preconditions
            var uriQuery = query != null ? "?" + query : "";
            var uriFragment = fragment != null ? "#" + fragment : null;

            // test execution
            var uri = new UriEndpoint($"{path}{uriQuery}{uriFragment}");

            Assert.Equal(expected, uri.ToString());
            Assert.Equal(path, !string.IsNullOrWhiteSpace(path)
                ? "/" + string.Join("/", uri.PathSegments.Skip(1))
                : path);
            Assert.Equal(query, uri.Query.Any() ? string.Join("&", uri.Query) : null);
            Assert.Equal(fragment, uri.Fragment);
        }

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

        /// <summary>
        /// Test the skip method.
        /// </summary>
        [Theory]
        [InlineData("/a/b/c", 0, "/a/b/c")]
        [InlineData("/a/b/c", 1, "/a/b/c")]
        [InlineData("/a/b/c", 2, "/b/c")]
        [InlineData("/a/b/c", 3, "/c")]
        [InlineData("/a/b/c", 4, null)]
        [InlineData("/a/b/c", 5, null)]
        public void Skip(string path, int skipCount, string expected)
        {
            // preconditions
            var uri = new UriEndpoint(path);

            // test execution
            var skip = uri.Skip(skipCount);

            Assert.Equal(expected, skip?.ToString());
        }

        /// <summary>
        /// Test the take method.
        /// </summary>
        [Theory]
        [InlineData("/a/b/c", 0, "/")]
        [InlineData("/a/b/c", 1, "/")]
        [InlineData("/a/b/c", 2, "/a")]
        [InlineData("/a/b/c", 3, "/a/b")]
        [InlineData("/a/b/c", 4, "/a/b/c")]
        [InlineData("/a/b/c", 5, "/a/b/c")]
        [InlineData("/a/b/c", -1, "/a/b")]
        [InlineData("/a/b/c", -2, "/a")]
        [InlineData("/a/b/c", -3, "/")]
        [InlineData("/a/b/c", -4, null)]
        [InlineData("/a/b/c", -5, null)]
        public void Take(string path, int takeCount, string expected)
        {
            // preconditions
            var uri = new UriEndpoint(path);

            // test execution
            var take = uri.Take(takeCount);

            Assert.Equal(expected, take?.ToString());
        }

        /// <summary>
        /// Test the extended path property.
        /// </summary>
        [Theory]
        [InlineData("http://user@example.com/x/y/z", "http://user@example.com/", "/x/y/z")]
        [InlineData("http://user@example.com/a/b/c/x/y/z", "http://user@example.com/a/b/c", "/x/y/z")]
        public void ExtendedPath(string uri, string endpointUri, string expected)
        {
            var resourceUri = new UriEndpoint(uri)
            {
                EndpointRoot = new UriEndpoint(endpointUri)
            };

            Assert.Equal(uri, resourceUri.ToString());
            Assert.Equal(expected, resourceUri.ExtendedPath.ToString());
        }
    }
}
