namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Allows authors to specify alternative media resources (e.g., different audio or video 
    /// formats) for media elements such as <video> or <audio>.
    /// </summary>
    public class HtmlElementEmbeddedSource : HtmlElement, IHtmlElementEmbedded
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementEmbeddedSource()
            : base("source", false)
        {
        }
    }
}
