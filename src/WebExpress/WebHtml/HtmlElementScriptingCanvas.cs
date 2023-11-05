using System;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Steht für einen Bitmap-Bereich, der von Skripts verwendet werden kann, um beispielsweise Diagramme, Spielegraphiken oder andere visuellen Effekte dynamisch darzustellen.
    /// </summary>
    public class HtmlElementScriptingCanvas : HtmlElement, IHtmlElementScripting
    {
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
        /// Constructor
        /// </summary>
        public HtmlElementScriptingCanvas()
            : base("canvas", false)
        {

        }
    }
}
