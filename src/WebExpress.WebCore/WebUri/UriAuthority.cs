using System;
using System.Linq;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// The authority (e.g. user@example.com:8080).
    /// </summary>
    public class UriAuthority
    {
        /// <summary>
        /// User information.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// The password.
        /// is deprecated in RFC 3986 (section 3.2.1).
        /// </summary>
        [Obsolete("The property is deprecated (see RFC 3986 Section 3.2.1).", false)]
        public string Password { get; set; }

        /// <summary>
        /// The host (e.g. example.com, 192.0.2.16:80).
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The port (e.g. example.com, 192.0.2.16:80).
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public UriAuthority()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">The host.</param>
        public UriAuthority(string host)
        {
            Host = host;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public UriAuthority(string host, int port)
        {
            Host = host;
            Port = port;
        }

        /// <summary>
        /// Converts the authority to a string.
        /// </summary>
        /// <returns>The string representation of the authority.</returns>
        public override string ToString()
        {
            return ToString(-1);
        }

        /// <summary>
        /// Converts the authority to a string.
        /// </summary>
        /// <param name="defaultPort">The default port for the active scheme.</param>
        /// <returns>The string representation of the authority.</returns>
        public virtual string ToString(int defaultPort)
        {
#pragma warning disable 618
            var userinfo = string.Join(":", new string[] { User, Password }.Where(x => !string.IsNullOrWhiteSpace(x)));
#pragma warning restore 618

            var adress = string.Join(":", new string[]
            {
                Host,
                Port != defaultPort ? Port?.ToString() : ""
            }.Where(x => !string.IsNullOrWhiteSpace(x)));

            return "//" + string.Join("@", new string[] { userinfo, adress }.Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}