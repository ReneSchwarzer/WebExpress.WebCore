using System.Linq;
using WebExpress.Core.WebUri;
using Xunit;

namespace WebExpress.Core.Test.Uri
{
    /// <summary>
    /// Tests an relative Uri.
    /// </summary>
    public class UnitTestUriRelative
    {
        [Fact]
        public void Test_0()
        {
            var str = "/abc#a?b=1&c=2";
            var uri = new UriResource(str);

            Assert.True
            (
                uri.ToString() == str &&
                uri.Scheme == UriScheme.Http &&
                uri.PathSegments.Count == 2 &&
                uri.Fragment == "a" &&
                uri.Query.FirstOrDefault()?.Key == "b" &&
                uri.Query.FirstOrDefault()?.Value == "1" &&
                uri.Query.LastOrDefault()?.Key == "c" &&
                uri.Query.LastOrDefault()?.Value == "2" &&
                uri.IsRelative
            );
        }

        [Fact]
        public void Test_1()
        {
            var str = "/assets/img/vila.svg";
            var uri = new UriResource("/assets/img/vila.svg");

            Assert.True
            (
                uri.ToString() == str &&
                uri.Scheme == UriScheme.Http &&
                uri.PathSegments.Count == 4 &&
                uri.Fragment == null &&
                uri.Query.Any() == false &&
                uri.IsRelative
            );
        }

        [Fact]
        public void Test_2()
        {
            var str = "/";
            var uri = new UriResource(str);

            Assert.True
            (
                uri.ToString() == str &&
                uri.Scheme == UriScheme.Http &&
                uri.Authority == null &&
                uri.PathSegments.Count == 1 &&
                uri.Fragment == null &&
                uri.Query.Any() == false &&
                uri.IsRelative
            );
        }

        [Fact]
        public void Test_3()
        {
            var str = "/?b=1&c=2";
            var uri = new UriResource(str);

            Assert.True
            (
                uri.ToString() == str &&
                uri.Scheme == UriScheme.Http &&
                uri.Authority == null &&
                uri.PathSegments.Count == 1 &&
                uri.Query.FirstOrDefault()?.Key == "b" &&
                uri.Query.FirstOrDefault()?.Value == "1" &&
                uri.Query.LastOrDefault()?.Key == "c" &&
                uri.Query.LastOrDefault()?.Value == "2" &&
                uri.IsRelative
            );
        }

        [Fact]
        public void Test_4()
        {
            var str = "";
            var uri = new UriResource(str);

            Assert.True
            (
                uri.ToString() == str + "/" &&
                uri.Scheme == UriScheme.Http &&
                uri.Authority == null &&
                uri.PathSegments.Count == 1 &&
                uri.Fragment == null &&
                uri.Query.Any() == false &&
                uri.IsRelative
            );
        }
    }
}
