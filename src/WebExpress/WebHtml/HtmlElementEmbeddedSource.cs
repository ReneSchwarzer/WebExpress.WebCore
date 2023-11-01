namespace WebExpress.WebHtml
{
    /// <summary>
    /// Ermöglicht es Autoren, alternative Medienressourcen (z.B. verschiedene Audio- oder Videoformate) 
    /// für Medienelemente wie <video> oder <audio> anzugeben.
    /// </summary>
    public class HtmlElementEmbeddedSource : HtmlElement, IHtmlElementEmbedded
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementEmbeddedSource()
            : base("source", false)
        {

        }
    }
}
