using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// see RFC 2616
    /// </summary>
    public class ResponseHeaderFields
    {
        /// <summary>
        /// Liefert oder setzt die Content-Länge
        /// </summary>
        public int ContentLength { get; set; }

        /// <summary>
        /// Liefert oder setzt den Content-Typ
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Liefert oder setzt die Sprache des Content
        /// </summary>
        public string ContentLanguage { get; set; }

        /// <summary>
        /// Liefert oder setzt die Direktiven für das Caching (see RFC 7234)
        /// </summary>
        public string CacheControl { get; set; }

        /// <summary>
        /// ContentDisposition
        /// </summary>
        public string ContentDisposition { get; set; }

        /// <summary>
        /// Die Basic Authentication (Basisauthentifizierung) nach RFC 2617
        /// </summary>
        public bool WWWAuthenticate { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Benutzerdefinierte Header
        /// </summary>
        public Dictionary<string, string> CustomHeader { get; private set; }

        /// <summary>
        /// Liefert oder setzt die Cookies
        /// </summary>
        public CookieCollection Cookies { get; } = new CookieCollection();

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
        /// Setzt ein benutzerdefinierten Header
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
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
        /// In Stringform umwandeln
        /// </summary>
        /// <returns></returns>
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
