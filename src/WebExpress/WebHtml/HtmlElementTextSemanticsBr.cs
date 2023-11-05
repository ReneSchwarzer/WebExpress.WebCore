namespace WebExpress.WebHtml
{
    /// <summary>
    /// Bezeichnet einen Zeilenumbruch.
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
