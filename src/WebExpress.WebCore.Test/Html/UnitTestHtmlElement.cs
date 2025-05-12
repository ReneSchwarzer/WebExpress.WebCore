using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.Test.Html
{
    /// <summary>
    /// Unit tests for the HtmlElementFieldLabel class.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestHtmlElement
    {
        /// <summary>
        /// Tests the find method.
        /// </summary>
        [Fact]
        public void FindSingel()
        {
            // preconditions
            var html = new HtmlElementTextContentDiv
            (
                new HtmlElementTextSemanticsI(),
                new HtmlElementTextSemanticsU(new HtmlElementTextSemanticsSpan()),
                new HtmlElementTextSemanticsB()
            );

            // test execution
            var res = html.Find(x => x is HtmlElementTextSemanticsSpan).FirstOrDefault();

            Assert.Equal(@"<span></span>", res.Trim());
        }

        /// <summary>
        /// Tests the find method.
        /// </summary>
        [Fact]
        public void Find()
        {
            // preconditions
            var html = new HtmlElement[]
            {
                new HtmlElementTextContentDiv
                (
                    new HtmlElementTextSemanticsI(),
                    new HtmlElementTextSemanticsU(new HtmlElementTextSemanticsSpan()),
                    new HtmlElementTextSemanticsB()
                ),
                new HtmlElementMultimediaImg()
            };

            // test execution
            var res = html.Find(x => x is HtmlElementTextSemanticsSpan).FirstOrDefault();

            Assert.Equal(@"<span></span>", res.Trim());
        }
    }
}
