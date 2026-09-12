using System.Xml.Serialization;

namespace WebExpress.WebCore.Config
{
    /// <summary>
    /// Optional configuration of the session and its cookie. The whole &lt;session&gt; element and
    /// every property in it is optional: a value left unset keeps the built-in default, so adding
    /// the block never changes behavior a deployment did not opt into.
    /// </summary>
    [XmlRoot("session", IsNullable = false)]
    public sealed class SessionConfig
    {
        /// <summary>
        /// The idle lifetime of a session in minutes, applied as a sliding window. It bounds both
        /// the server-side session and the cookie's lifetime. Left unset, the built-in default
        /// applies; a non-positive value disables expiry, which also turns the cookie into a
        /// session cookie that dies when the browser closes rather than one that never expires.
        /// </summary>
        [XmlElement("timeout")]
        public int? TimeoutMinutes { get; set; }

        /// <summary>
        /// Forces the <c>Secure</c> flag on the session cookie on or off. Left unset, the flag
        /// tracks the request scheme - set for an https request, absent for plain http - which is
        /// the right default behind most deployments. Set it to <c>true</c> when the server runs
        /// behind a TLS-terminating proxy and therefore sees plain http itself, so the cookie is
        /// still marked https-only towards the browser.
        /// </summary>
        [XmlElement("secure")]
        public bool? Secure { get; set; }
    }
}
