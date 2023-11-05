using WebExpress.WebUri;
using Xunit;

namespace WebExpress.Test.Uri
{
    /// <summary>
    /// Tests the append method.
    /// </summary>
    public class UnitTestUriRelativeAppend
    {
        private readonly UriResource Uri = new UriResource("/a/b/c");

        [Fact]
        public void Append_0()
        {
            var append = Uri.Append("/d");

            Assert.True
            (
                append.ToString() == "/a/b/c/d" &&
                append.PathSegments.Count == 5
            );
        }

        [Fact]
        public void Append_1()
        {
            var append = Uri.Append("d");

            Assert.True
            (
                append.ToString() == "/a/b/c/d" &&
                append.PathSegments.Count == 5
            );
        }

        [Fact]
        public void Append_2()
        {
            var append = Uri.Append("/d/e/f");

            Assert.True
            (
                append.ToString() == "/a/b/c/d/e/f" &&
                append.PathSegments.Count == 7
            );
        }
    }
}
