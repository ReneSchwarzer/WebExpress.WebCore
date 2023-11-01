namespace WebExpress.WebHtml
{
    /// <summary>
    /// Wird verwendet, um externe JavaScript- und CSS-Dateien in das aktuelle HTML-Dokument einzubinden.
    /// </summary>
    public class HtmlElementMetadataLink : HtmlElement, IHtmlElementMetadata
    {
        /// <summary>
        /// Liefert oder setzt die Url
        /// </summary>
        public string Href
        {
            get => GetAttribute("href");
            set => SetAttribute("href", value);
        }

        /// <summary>
        /// Returns or sets the type.
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
