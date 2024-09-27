using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.Test.Html
{
    [Collection("NonParallelTests")]
    public class UnitTestHtmlText
    {
        /// <summary>
        /// Tests a empty tag.
        /// </summary>
        [Fact]
        public void Empty()
        {
            // test execution
            var html = new HtmlText();

            Assert.Null(html.Value);
        }

        /// <summary>
        /// Tests a tag.
        /// </summary>
        [Fact]
        public void TextAtInstancing()
        {
            // test execution
            var html = new HtmlText("abcdef");

            Assert.Equal(@"abcdef", html.Value);
        }

        /// <summary>
        /// Tests a tag.
        /// </summary>
        [Fact]
        public void TextAtProperty()
        {
            // test execution
            var html = new HtmlText
            {
                Value = "abcdef"
            };

            Assert.Equal(@"abcdef", html.Value);
        }
    }
}
