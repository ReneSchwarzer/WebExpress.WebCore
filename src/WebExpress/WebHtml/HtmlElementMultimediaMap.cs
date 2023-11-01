namespace WebExpress.WebHtml
{
    /// <summary>
    /// Definiert in Verbindung mit dem <area>-Element eine Image Map.
    /// </summary>
    public class HtmlElementMultimediaMap : HtmlElement, IHtmlElementMultimedia
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMultimediaMap()
            : base("map", false)
        {

        }
    }
}
