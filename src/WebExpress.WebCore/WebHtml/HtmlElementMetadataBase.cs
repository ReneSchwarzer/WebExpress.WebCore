namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Provides the basis for relative references. 
    /// </summary>
    public class HtmlElementMetadataBase : HtmlElement, IHtmlElementMetadata
    {
        /// <summary>
        /// Returns or sets the destination uri.
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
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementMetadataBase()
            : base("base")
        {
            CloseTag = false;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="url">The uri.</param>
        public HtmlElementMetadataBase(string url)
            : this()
        {
            Href = url;
        }
    }
}
