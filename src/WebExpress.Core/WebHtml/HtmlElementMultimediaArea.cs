namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Represents an image map in conjunction with the <map>element.
    /// </summary>
    public class HtmlElementMultimediaArea : HtmlElement, IHtmlElementMultimedia
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMultimediaArea()
            : base("area", false)
        {

        }
    }
}
