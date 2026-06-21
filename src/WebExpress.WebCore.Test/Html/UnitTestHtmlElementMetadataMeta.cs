using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.Test.Html
{
    /// <summary>
    /// Unit tests for the HtmlElementMetadataMeta class.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestHtmlElementMetadataMeta
    {
        /// <summary>
        /// Tests a tag built from a key and a value. Meta is rendered as a single
        /// key/value attribute, so the empty constructor has no meaningful output.
        /// </summary>
        [Fact]
        public void KeyValue()
        {
            // act
            var html = new HtmlElementMetadataMeta("charset", "utf-8");

            Assert.Equal(@"<meta charset='utf-8'>", html.Trim());
        }
    }
}
