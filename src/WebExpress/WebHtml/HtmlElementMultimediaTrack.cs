namespace WebExpress.WebHtml
{
    /// <summary>
    /// Hiermit können zusätzliche Medienspuren (z.B. Untertitel) für Elemente wie <video> oder<audio> angegeben werden. 
    /// </summary>
    public class HtmlElementMultimediaTrack : HtmlElement, IHtmlElementMultimedia
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMultimediaTrack()
            : base("track", false)
        {

        }
    }
}
