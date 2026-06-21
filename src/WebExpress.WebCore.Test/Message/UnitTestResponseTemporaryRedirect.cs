using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.Test.Message
{
    /// <summary>
    /// Unit tests for the ResponseTemporaryRedirect class.
    /// </summary>
    public class UnitTestResponseTemporaryRedirect
    {
        /// <summary>
        /// Tests that the status code is derived from the StatusCode attribute.
        /// </summary>
        [Fact]
        public void StatusCode()
        {
            // act
            var response = new ResponseTemporaryRedirect();

            // validation
            Assert.Equal(307, response.Status);
        }

        /// <summary>
        /// Tests that the target location is written to the Location header.
        /// </summary>
        [Fact]
        public void Location()
        {
            // arrange
            var location = new UriEndpoint("/target");

            // act
            var response = new ResponseTemporaryRedirect(location);

            // validation
            Assert.Equal(location.ToString(), response.Header.Location);
        }
    }
}
