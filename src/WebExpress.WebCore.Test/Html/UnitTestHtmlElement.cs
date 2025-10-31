using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.Test.Html
{
    /// <summary>
    /// Unit tests for the HtmlElement class.
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

            // validation
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

            // validation
            Assert.Equal(@"<span></span>", res.Trim());
        }

        /// <summary>
        /// Tests the AddClass method.
        /// </summary>
        [Fact]
        public void AddClassTest()
        {
            // preconditions
            var div = new HtmlElementTextContentDiv();

            // test execution
            div.AddClass("test-class");

            // validation
            Assert.Contains("class=\"test-class\"", div.ToString());
        }

        /// <summary>
        /// Tests the RemoveClass method.
        /// </summary>
        [Fact]
        public void RemoveClassTest()
        {
            // preconditions
            var div = new HtmlElementTextContentDiv();
            div.AddClass("test-class");

            // test execution
            div.RemoveClass("test-class");

            // validation
            Assert.DoesNotContain("class=\"test-class\"", div.ToString());
        }

        /// <summary>
        /// Tests the AddStyle method.
        /// </summary>
        [Fact]
        public void AddStyleTest()
        {
            // preconditions
            var div = new HtmlElementTextContentDiv();

            // test execution
            div.AddStyle("color:red;");

            // validation
            Assert.Contains("style=\"color:red;\"", div.ToString());
        }

        /// <summary>
        /// Tests the RemoveStyle method.
        /// </summary>
        [Fact]
        public void RemoveStyleTest()
        {
            // preconditions
            var div = new HtmlElementTextContentDiv();
            div.AddStyle("color", "red");

            // test execution
            div.RemoveStyle("color");

            // validation
            Assert.DoesNotContain("style=\"color:red;\"", div.ToString());
        }

        /// <summary>
        /// Tests adding multiple CSS classes.
        /// </summary>
        [Fact]
        public void AddMultipleClassesTest()
        {
            // preconditions
            var div = new HtmlElementTextContentDiv();

            // test execution
            div.AddClass("class1");
            div.AddClass("class2");

            // validation
            Assert.Contains("class=\"class1 class2\"", div.ToString());
        }

        /// <summary>
        /// Tests removing one of multiple CSS classes.
        /// </summary>
        [Fact]
        public void RemoveOneOfMultipleClassesTest()
        {
            // preconditions
            var div = new HtmlElementTextContentDiv();
            div.AddClass("class1");
            div.AddClass("class2");

            // test execution
            div.RemoveClass("class1");

            // validation
            Assert.DoesNotContain("class1", div.ToString());
            Assert.Contains("class2", div.ToString());
        }

        /// <summary>
        /// Tests adding multiple styles.
        /// </summary>
        [Fact]
        public void AddMultipleStylesTest()
        {
            // preconditions
            var div = new HtmlElementTextContentDiv();

            // test execution
            div.AddStyle("color:red;");
            div.AddStyle("background:blue;");

            // validation
            Assert.Contains("color:red;", div.ToString());
            Assert.Contains("background:blue;", div.ToString());
        }

        /// <summary>
        /// Tests removing one of multiple styles.
        /// </summary>
        [Fact]
        public void RemoveOneOfMultipleStylesTest()
        {
            // preconditions
            var div = new HtmlElementTextContentDiv();
            div.AddStyle("color:red;");
            div.AddStyle("background:blue;");

            // test execution
            div.RemoveStyle("color:red;");

            // validation
            Assert.DoesNotContain("color:red;", div.ToString());
            Assert.Contains("background:blue;", div.ToString());
        }

        /// <summary>
        /// Tests that ToString returns the correct HTML for an empty div.
        /// </summary>
        [Fact]
        public void ToStringEmptyDivTest()
        {
            // preconditions
            var div = new HtmlElementTextContentDiv();

            // validation
            Assert.Equal("<div></div>", div.ToString().Trim());
        }

        /// <summary>
        /// Tests that ToString returns the correct HTML for a div with child elements.
        /// </summary>
        [Fact]
        public void ToStringWithChildrenTest()
        {
            // preconditions
            var div = new HtmlElementTextContentDiv(
                new HtmlElementTextSemanticsB(),
                new HtmlElementTextSemanticsI()
            );

            // validation
            Assert.Contains("<b></b>", div.ToString());
            Assert.Contains("<i></i>", div.ToString());
        }

    }
}
