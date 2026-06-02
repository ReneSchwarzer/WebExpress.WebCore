using System;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents an image.
    /// </summary>
    public class HtmlElementMultimediaImg : HtmlElement, IHtmlElementMultimedia
    {
        /// <summary>
        /// Gets or sets the alternate text., wenn das Bild nicht angezeigt werden kann
        /// </summary>
        public string Alt
        {
            get => GetAttribute("alt");
            set => SetAttribute("alt", value);
        }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        public string Title
        {
            get => GetAttribute("title");
            set => SetAttribute("title", value);
        }

        /// <summary>
        /// Gets or sets the image uri.
        /// </summary>
        public string Src
        {
            get => GetAttribute("src");
            set => SetAttribute("src", value);
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width
        {
            get => int.TryParse(GetAttribute("width"), out var width) ? width : 0;
            set => SetAttribute("width", value.ToString());
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Height
        {
            get => int.TryParse(GetAttribute("height"), out var height) ? height : 0;
            set => SetAttribute("height", value.ToString());
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        public string Target
        {
            get => GetAttribute("target");
            set => SetAttribute("target", value);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementMultimediaImg()
            : base("img", false)
        {
        }
    }
}
