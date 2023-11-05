namespace WebExpress.WebHtml
{
    /// <summary>
    /// Hiermit kann die Gelegenheit für einen Zeilenumbruch gekennzeichnet werden, mit dem die Lesbarkeit verbessert werden kann, wenn The text. auf mehrere Zeilen verteilt wird.
    /// </summary>
    public class HtmlElementTextSemanticsWbr : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTextSemanticsWbr()
            : base("wbr", false)
        {
        }
    }
}
