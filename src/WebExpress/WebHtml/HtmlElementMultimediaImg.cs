using System;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Steht für ein Bild. 
    /// </summary>
    public class HtmlElementMultimediaImg : HtmlElement, IHtmlElementMultimedia
    {
        /// <summary>
        /// Liefert oder setzt den alternativen Text, wenn das Bild nicht angezeigt werden kann
        /// </summary>
        public string Alt
        {
            get => GetAttribute("alt");
            set => SetAttribute("alt", value);
        }

        /// <summary>
        /// Returns or sets the tooltip.
        /// </summary>
        public string Title
        {
            get => GetAttribute("title");
            set => SetAttribute("title", value);
        }

        /// <summary>
        /// Liefert oder setzt die Bild-Url
        /// </summary>
        public string Src
        {
            get => GetAttribute("src");
            set => SetAttribute("src", value);
        }

        /// <summary>
        /// Returns or sets the width.
        /// </summary>
        public int Width
        {
            get => Convert.ToInt32(GetAttribute("width"));
            set => SetAttribute("width", value.ToString());
        }

        /// <summary>
        /// Returns or sets the width.
        /// </summary>
        public int Height
        {
            get => Convert.ToInt32(GetAttribute("height"));
            set => SetAttribute("height", value.ToString());
        }

        /// <summary>
        /// Liefert oder setzt das Ziel
        /// </summary>
        public string Target
        {
            get => GetAttribute("target");
            set => SetAttribute("target", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMultimediaImg()
            : base("img", false)
        {

        }
    }
}
