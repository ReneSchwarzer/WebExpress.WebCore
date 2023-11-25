using WebExpress.WebCore.WebUri;
using Xunit;

namespace WebExpress.WebCore.Test.Uri
{
    /// <summary>
    /// Tests the skip method.
    /// </summary>
    public class UnitTestUriRelativeSkip
    {
        private readonly UriResource Uri = new UriResource("/a/b/c");

        [Fact]
        public void Skip_0()
        {
            var skip = Uri.Skip(0);

            Assert.True
            (
                skip.ToString().Equals("/a/b/c") && skip.PathSegments.Count == 4
            );
        }

        [Fact]
        public void Skip_1()
        {
            var skip = Uri.Skip(1);

            Assert.True
            (
                skip.ToString().Equals("/a/b/c") && skip.PathSegments.Count == 3
            );
        }

        [Fact]
        public void Skip_2()
        {
            var skip = Uri.Skip(2);

            Assert.True
            (
                skip.ToString().Equals("/b/c") && skip.PathSegments.Count == 2
            );
        }

        [Fact]
        public void Skip_3()
        {
            var skip = Uri.Skip(3);

            Assert.True
            (
                skip.ToString().Equals("/c") && skip.PathSegments.Count == 1
            );
        }

        [Fact]
        public void Skip_4()
        {
            var skip = Uri.Skip(4);

            Assert.True
            (
                skip == null
            );
        }

        [Fact]
        public void Skip_5()
        {
            var skip = new UriResource().Skip(1);

            Assert.True
            (
                skip == null
            );
        }
    }
}
