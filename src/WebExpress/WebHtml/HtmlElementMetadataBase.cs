namespace WebExpress.WebHtml
{
    /// <summary>
    /// Stellt die Basis für relative Verweise da. 
    /// </summary>
    public class HtmlElementMetadataBase : HtmlElement, IHtmlElementMetadata
    {
        /// <summary>
        /// Liefert oder setzt die Ziel-Url
        /// </summary>
        public string Href
        {
            get => GetAttribute("href");
            set
            {
                var url = value;

                if (!string.IsNullOrWhiteSpace(url) && !url.EndsWith("/"))
                {
                    url += url + "/";
                }

                SetAttribute("href", url);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMetadataBase()
            : base("base")
        {
            CloseTag = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">The uri.</param>
        public HtmlElementMetadataBase(string url)
            : this()
        {
            Href = url;
        }
    }
}
