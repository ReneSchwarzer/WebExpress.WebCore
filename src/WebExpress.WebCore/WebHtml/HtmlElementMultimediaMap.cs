namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents an image map in conjunction with the <area>element.
    /// </summary>
    public class HtmlElementMultimediaMap : HtmlElement, IHtmlElementMultimedia
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMultimediaMap()
            : base("map", false)
        {
        }
    }
}
