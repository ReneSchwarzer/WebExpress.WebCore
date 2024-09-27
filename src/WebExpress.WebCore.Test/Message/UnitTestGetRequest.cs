using WebExpress.WebCore.Test.Fixture;

namespace WebExpress.WebCore.Test.Message
{
    /// <summary>
    /// UnitTestGetRequest class for testing HTTP GET requests.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestGetRequest
    {
        /// <summary>
        /// Tests a general GET request.
        /// </summary>
        [Fact]
        public void General()
        {
            var content = UnitTestControlFixture.GetEmbeddedResource("general.get");
            var request = UnitTestControlFixture.CrerateRequest(content);

            Assert.Equal("http://localhost:8080/abc/xyz/A7BCCCA9-4C7E-4117-9EE2-ECC3381B605A", request.Uri?.ToString());
        }

        /// <summary>
        /// Tests a GET request with less data.
        /// </summary>
        [Fact]
        public void Less()
        {
            var content = UnitTestControlFixture.GetEmbeddedResource("less.get");
            var request = UnitTestControlFixture.CrerateRequest(content);

            Assert.Equal("http://localhost:8080/abc/xyz/A7BCCCA9-4C7E-4117-9EE2-ECC3381B605A", request.Uri?.ToString());
        }

        /// <summary>
        /// Tests a GET request with massive data.
        /// </summary>
        [Fact]
        public void Massive()
        {
            var content = UnitTestControlFixture.GetEmbeddedResource("massive.get");
            var request = UnitTestControlFixture.CrerateRequest(content);

            Assert.Equal("http://localhost:8080/abc/xyz/A7BCCCA9-4C7E-4117-9EE2-ECC3381B605A", request.Uri?.ToString());
        }

        /// <summary>
        /// Tests a GET request with parameters.
        /// </summary>
        [Fact]
        public void GetParameter()
        {
            var content = UnitTestControlFixture.GetEmbeddedResource("param.get");
            var request = UnitTestControlFixture.CrerateRequest(content);
            var param = request?.GetParameter("a")?.Value;

            Assert.Equal("http://localhost:8080/abc/xyz/A7BCCCA9-4C7E-4117-9EE2-ECC3381B605A", request.Uri?.ToString());
            Assert.Equal("1", param);
        }

        /// <summary>
        /// Tests a GET request with parameters containing umlauts.
        /// </summary>
        [Fact]
        public void GetParameterWithUmlaut()
        {
            var content = UnitTestControlFixture.GetEmbeddedResource("param_umlaut.get");
            var request = UnitTestControlFixture.CrerateRequest(content);
            var a = request?.GetParameter("a")?.Value;
            var b = request?.GetParameter("b")?.Value;

            Assert.Equal("http://localhost:8080/abc/xyz/A7BCCCA9-4C7E-4117-9EE2-ECC3381B605A", request.Uri?.ToString());
            Assert.Equal("ä", a);
            Assert.Equal("ö ü", b);
        }
    }
}
