namespace WebExpress.Core.WebHtml
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
