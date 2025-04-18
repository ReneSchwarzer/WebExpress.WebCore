using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents the response header fields as per RFC 2616.
    /// </summary>
    public class ResponseHeaderFields
    {
        /// <summary>
        /// Returns or sets the content length.
        /// </summary>
        public int ContentLength { get; set; }

        /// <summary>
        /// Returns or sets the content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Returns or sets the content language.
        /// </summary>
        public string ContentLanguage { get; set; }

        /// <summary>
        /// Returns or sets the cache control directives (see RFC 7234).
        /// </summary>
        public string CacheControl { get; set; }

        /// <summary>
        /// Returns or sets the content disposition.
        /// </summary>
        public string ContentDisposition { get; set; }

        /// <summary>
        /// Returns or sets a value indicating whether basic authentication (as per RFC 2617) is required.
        /// </summary>
        public bool WWWAuthenticate { get; set; }

        /// <summary>
        /// Returns or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Returns the custom headers.
        /// </summary>
        public IDictionary<string, string> CustomHeader { get; private set; }

        /// <summary>
        /// Returns the cookies.
        /// </summary>
        public CookieCollection Cookies { get; } = [];

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseHeaderFields()
        {
            CustomHeader = new Dictionary<string, string>();
            WWWAuthenticate = false;
            ContentLength = -1;
        }

        /// <summary>
        /// Adds a custom header.
        /// </summary>
        /// <param name="key">The header key.</param>
        /// <param name="value">The header value.</param>
        public void AddCustomHeader(string key, string value)
        {
            if (!CustomHeader.ContainsKey(key))
            {
                CustomHeader.Add(key, value);
            }
            else
            {
                CustomHeader[key] = value;
            }
        }

        /// <summary>
        /// Converts the response header fields to a string representation.
        /// </summary>
        /// <returns>A string representation of the response header fields.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(ContentType))
            {
                sb.AppendLine("Content-Type: " + ContentType);
            }

            if (ContentLength > -1)
            {
                sb.AppendLine("Content-Length:" + ContentLength);
            }

            if (!string.IsNullOrWhiteSpace(ContentDisposition))
            {
                sb.AppendLine("Content-Disposition: " + ContentDisposition);
            }

            if (!string.IsNullOrWhiteSpace(CacheControl))
            {
                sb.AppendLine("Cache-Control: " + CacheControl);
            }

            if (WWWAuthenticate)
            {
                sb.AppendLine("WWW-Authenticate: Basic realm=\"Bereich\"");
            }

            if (!string.IsNullOrWhiteSpace(Location))
            {
                sb.AppendLine("Location: " + Location);
            }

            foreach (var c in CustomHeader)
            {
                sb.AppendLine(c.Key + ": " + c.Value);
            }

            return sb.ToString();
        }
    }
}
