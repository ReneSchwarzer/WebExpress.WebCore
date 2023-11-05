namespace WebExpress.WebHtml
{
    /// <summary>
    /// Steht für eine Videodatei und die dazugehörigen Audiodateien, sowie die für das Abspielen nötigen Kontrollelemente.
    /// </summary>
    public class HtmlElementMultimediaVideo : HtmlElement, IHtmlElementMultimedia
    {
        /// <summary>
        /// Liefert oder setzt die Video-Url
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
