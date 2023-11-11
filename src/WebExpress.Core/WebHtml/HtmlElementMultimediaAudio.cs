namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Represents a sound file or audio stream.
    /// </summary>
    public class HtmlElementMultimediaAudio : HtmlElement, IHtmlElementMultimedia
    {
        /// <summary>
        /// Liefert oder setzt die Audio-Url
        /// </summary>
        public string Src
        {
            get => GetAttribute("src");
            set => SetAttribute("src", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMultimediaAudio()
            : base("audio", false)
        {
        }
    }
}
