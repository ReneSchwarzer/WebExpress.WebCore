namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a line break.
    /// </summary>
    public class HtmlElementTextSemanticsBr : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTextSemanticsBr()
            : base("br", false)
        {
        }
    }
}
