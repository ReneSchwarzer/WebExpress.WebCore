namespace WebExpress.WebHtml
{
    /// <summary>
    /// Markiert eine Tondatei oder einen Audiostream.
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
