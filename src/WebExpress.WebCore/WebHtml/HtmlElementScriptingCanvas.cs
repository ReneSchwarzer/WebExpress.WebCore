using System;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents an area of bitmap that can be used by scripts to dynamically display 
    /// diagrams, game graphics, or other visual effects, for example.
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
