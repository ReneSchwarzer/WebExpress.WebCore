using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.Test.Uri
{
    /// <summary>
    /// Tests the take method.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestUriRelativeTake(UnitTestControlFixture fixture) : IClassFixture<UnitTestControlFixture>
    {
        private readonly UriResource Uri = new UriResource("/a/b/c");

        [Fact]
        public void Take_0()
        {
            var take = Uri.Take(0);

            Assert.True
            (
                take.ToString().Equals("/") && take.PathSegments.Count == 0
            );
        }

        [Fact]
        public void Take_1()
        {
            var take = Uri.Take(1);

            Assert.True
            (
                take.ToString().Equals("/") && take.PathSegments.Count == 1
            );
        }

        [Fact]
        public void Take_2()
        {
            var take = Uri.Take(2);

            Assert.True
            (
                take.ToString().Equals("/a") && take.PathSegments.Count == 2
            );
        }

        [Fact]
        public void Take_3()
        {
            var take = Uri.Take(3);

            Assert.True
            (
                take.ToString().Equals("/a/b") && take.PathSegments.Count == 3
            );
        }

        [Fact]
        public void Take_4()
        {
            var take = Uri.Take(4);

            Assert.True
            (
                take.ToString().Equals("/a/b/c") && take.PathSegments.Count == 4
            );
        }

        [Fact]
        public void Take_5()
        {
            var take = Uri.Take(5);

            Assert.True
            (
                take.ToString().Equals("/a/b/c") && take.PathSegments.Count == 4
            );
        }

        [Fact]
        public void Take_6()
        {
            var take = Uri.Take(-1);

            Assert.True
            (
                take.ToString().Equals("/a/b") && take.PathSegments.Count == 3
            );
        }

        [Fact]
        public void Take_7()
        {
            var take = Uri.Take(-2);

            Assert.True
            (
                take.ToString().Equals("/a") && take.PathSegments.Count == 2
            );
        }

        [Fact]
        public void Take_8()
        {
            var take = Uri.Take(-3);

            Assert.True
            (
                take.ToString().Equals("/") && take.PathSegments.Count == 1
            );
        }

        [Fact]
        public void Take_9()
        {
            var take = Uri.Take(-4);

            Assert.True
            (
                take == null
            );
        }

        [Fact]
        public void Take_10()
        {
            var take = Uri.Take(-5);

            Assert.True
            (
                take == null
            );
        }
    }
}
