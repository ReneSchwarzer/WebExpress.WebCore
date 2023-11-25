using System.Xml.Serialization;

namespace WebExpress.WebCore.Config
{
    /// <summary>
    /// Class for reading certificate properties.
    /// </summary>
    [XmlRoot("endpoint", IsNullable = false)]
    public sealed class EndpointConfig
    {
        /// <summary>
        /// The uri (e.g. https://localhost:443/).
        /// </summary>
        [XmlAttribute("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// The certificate as a pfx file.
        /// </summary>
        [XmlAttribute("pfx")]
        public string PfxFile { get; set; }

        /// <summary>
        /// The password.
        /// </summary>
        [XmlAttribute("password")]
        public string Password { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EndpointConfig()
        {
        }
    }
}