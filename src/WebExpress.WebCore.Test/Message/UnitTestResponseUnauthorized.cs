using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.Test.Message
{
    /// <summary>
    /// Unit tests for the ResponseUnauthorized class.
    /// </summary>
    public class UnitTestResponseUnauthorized
    {
        /// <summary>
        /// Tests that the status code is derived from the StatusCode attribute.
        /// </summary>
        [Fact]
        public void StatusCode()
        {
            // act
            var response = new ResponseUnauthorized();

            // validation
            Assert.Equal(401, response.Status);
        }

        /// <summary>
        /// Tests that the WWW-Authenticate header is requested so the client is prompted to authenticate.
        /// </summary>
        [Fact]
        public void WwwAuthenticate()
        {
            // act
            var response = new ResponseUnauthorized();

            // validation
            Assert.True(response.Header.WWWAuthenticate);
        }
    }
}
