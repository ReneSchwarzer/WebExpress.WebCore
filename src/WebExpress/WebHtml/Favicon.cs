namespace WebExpress.WebHtml
{
    public class Favicon
    {
        /// <summary>
        /// Returns or sets the uri.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Returns or sets the media type.
        /// </summary>
        public TypeFavicon Mediatype { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">The uri.</param>
        /// <param name="mediatype">The media type.</param>
        public Favicon(string url, TypeFavicon mediatype)
        {
            Url = url;
            Mediatype = mediatype;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">The uri.</param>
        /// <param name="mediatype">The media type.</param>
        public Favicon(string url)
        {
            Url = url;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">The uri.</param>
        /// <param name="mediatype">The media type.</param>
        public Favicon(string url, string mediatype)
        {
            Url = url;

            switch (mediatype)
            {
                case "image/x-icon":
                    Mediatype = TypeFavicon.ICON;
                    break;
                case "image/jpg":
                    Mediatype = TypeFavicon.JPG;
                    break;
                case "image/png":
                    Mediatype = TypeFavicon.PNG;
                    break;
                case "image/svg+xml":
                    Mediatype = TypeFavicon.SVG;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Returns the media type.
        /// </summary>
        /// <returns>The media type.</returns>
        public string GetMediatyp()
        {
            return Mediatype switch
            {
                TypeFavicon.ICON => "image/x-icon",
                TypeFavicon.JPG => "image/jpg",
                TypeFavicon.PNG => "image/png",
                TypeFavicon.SVG => "image/svg+xml",
                _ => "",
            };
        }
    }
}
