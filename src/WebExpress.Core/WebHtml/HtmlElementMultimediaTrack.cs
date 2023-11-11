namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Allows you to specify additional media tracks (e.g. subtitles) for elements such as <video> or <audio>. 
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
