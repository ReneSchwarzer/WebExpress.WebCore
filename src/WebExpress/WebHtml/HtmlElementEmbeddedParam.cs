namespace WebExpress.WebHtml
{
    /// <summary>
    /// Definiert Parameter für ein Plugin, das für die Darstellung eines 
    /// mit <object> eingebundenen Elements verwendet werden.
    /// </summary>
    public class HtmlElementEmbeddedParam : HtmlElement, IHtmlElementEmbedded
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementEmbeddedParam()
            : base("param", false)
        {

        }
    }
}
