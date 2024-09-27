using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.Test.Html
{
    [Collection("NonParallelTests")]
    public class UnitTestHtmlElementTextContentP
    {
        /// <summary>
        /// Tests a empty tag.
        /// </summary>
        [Fact]
        public void Empty()
        {
            // test execution
            var html = new HtmlElementTextContentP();

            Assert.Equal(@"<p></p>", html.Trim());
        }

        /// <summary>
        /// Tests a tag.
        /// </summary>
        [Fact]
        public void TextAtInstancing()
        {
            // test execution
            var html = new HtmlElementTextContentP("abcdef");

            Assert.Equal(@"<p>abcdef</p>", html.Trim());
        }

        /// <summary>
        /// Tests a tag.
        /// </summary>
        [Fact]
        public void TextAtProperty()
        {
            // test execution
            var html = new HtmlElementTextContentP
            {
                Text = "abcdef"
            };

            Assert.Equal(@"<p>abcdef</p>", html.Trim());
        }

        /// <summary>
        /// Tests a tag.
        /// </summary>
        [Fact]
        public void TextAtHtmlText()
        {
            // test execution
            var html = new HtmlElementTextContentP(new HtmlText("abc"), new HtmlText("def"));
            var str = html.ToString();

            Assert.Equal(@"<p>abcdef</p>", html.Trim());
        }

        /// <summary>
        /// Tests a tag.
        /// </summary>
        [Fact]
        public void TextWithId()
        {
            // test execution
            var html = new HtmlElementTextContentP()
            {
                Id = "identity"
            };

            Assert.Equal(@"<p id=""identity""></p>", html.Trim());
        }

        /// <summary>
        /// Tests a tag.
        /// </summary>
        [Fact]
        public void Inline()
        {
            // test execution
            var html = new HtmlElementTextContentP();

            Assert.False(html.Inline);
        }

        /// <summary>
        /// Tests a tag.
        /// </summary>
        [Fact]
        public void CloseTag()
        {
            // test execution
            var html = new HtmlElementTextContentP();

            Assert.True(html.CloseTag);
        }

        /// <summary>
        /// Tests a tag.
        /// </summary>
        [Fact]
        public void Class()
        {
            // test execution
            var html = new HtmlElementTextContentP()
            {
                Class = "abc"
            };

            Assert.Equal(@"<p class=""abc""></p>", html.Trim());
        }

        /// <summary>
        /// Tests a tag.
        /// </summary>
        [Fact]
        public void Style()
        {
            // test execution
            var html = new HtmlElementTextContentP()
            {
                Style = "abc"
            };

            Assert.Equal(@"<p style=""abc""></p>", html.Trim());
        }
    }
}
