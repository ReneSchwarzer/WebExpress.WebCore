namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// This can be used to mark the opportunity for a line break, which can be used to 
    /// improve readability when the text is spread over multiple lines.
    /// </summary>
    public class HtmlElementTextSemanticsWbr : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTextSemanticsWbr()
            : base("wbr", false)
        {
        }
    }
}
