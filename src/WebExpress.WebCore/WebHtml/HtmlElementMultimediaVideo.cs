namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a video file and its audio files, as well as the controls needed to play it.
    /// </summary>
    public class HtmlElementMultimediaVideo : HtmlElement, IHtmlElementMultimedia
    {
        /// <summary>
        /// Returns or sets the video uri.
        /// </summary>
        public string Src
        {
            get => GetAttribute("src");
            set => SetAttribute("src", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMultimediaVideo()
            : base("video", false)
        {

        }
    }
}
