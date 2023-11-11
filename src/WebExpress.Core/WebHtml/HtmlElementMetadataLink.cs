namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Used to include external java script and css files in the current html document.
    /// </summary>
    public class HtmlElementMetadataLink : HtmlElement, IHtmlElementMetadata
    {
        /// <summary>
        /// Returns or sets the uri.
        /// </summary>
        public string Href
        {
            get => GetAttribute("href");
            set => SetAttribute("href", value);
        }

        /// <summary>
        /// Returns or sets the rel.
        /// </summary>
        public string Rel
        {
            get => GetAttribute("rel");
            set => SetAttribute("rel", value);
        }

        /// <summary>
        /// Returns or sets the type.
        /// </summary>
        public string Type
        {
            get => GetAttribute("type");
            set => SetAttribute("type", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMetadataLink()
            : base("link", false)
        {
        }
    }
}
