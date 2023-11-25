namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a line break.
    /// </summary>
    public class HtmlElementTextSemanticsBr : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTextSemanticsBr()
            : base("br", false)
        {
        }
    }
}
