using System;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents an image.
    /// </summary>
    public class HtmlElementMultimediaImg : HtmlElement, IHtmlElementMultimedia
    {
        /// <summary>
        /// Returns or sets the alternate text., wenn das Bild nicht angezeigt werden kann
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
        /// Returns or sets the image uri.
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
        /// Returns or sets the target.
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
