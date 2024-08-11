namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Allows you to specify additional media tracks (e.g. subtitles) for elements such as <video> or <audio>. 
    /// </summary>
    public class HtmlElementMultimediaTrack : HtmlElement, IHtmlElementMultimedia
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementMultimediaTrack()
            : base("track", false)
        {
        }
    }
}
