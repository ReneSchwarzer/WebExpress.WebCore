using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.Test.Html
{
    /// <summary>
    /// Unit tests for the HtmlElementMetadataBase class.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestHtmlElementMetadataBase
    {
        /// <summary>
        /// Tests the constructor of the HtmlElementMetadataBase class.
        /// </summary>
        [Fact]
        public void Constructor()
        {
            // preconditions
            var url = "https://example.com";

            // test execution
            var element = new HtmlElementMetadataBase(url);

            // validation
            Assert.Equal("https://example.com", element.Href);
        }

        /// <summary>
        /// Sets the URI for the HTML element and validates its format.
        /// </summary>
        [Theory]
        [InlineData(null, "")]
        [InlineData("https://example.com", "https://example.com")]
        public void Href(string uri, string expected)
        {
            // preconditions
            var element = new HtmlElementMetadataBase();

            // test execution
            element.Href = uri;

            // validation
            Assert.Equal(expected, element.Href);
        }
    }
}
