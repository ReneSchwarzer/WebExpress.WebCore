using System.Collections.Generic;
using System.Xml.Serialization;
using WebExpress.WebCore.Setting;

namespace WebExpress.WebCore.Config
{
    /// <summary>
    /// Class for reading the configuration file.
    /// </summary>
    [XmlRoot("config", IsNullable = false)]
    public sealed class HttpServerConfig
    {
        /// <summary>
        /// The configuration version.
        /// </summary>
        [XmlAttribute("version", DataType = "int")]
        public int Version { get; set; }

        /// <summary>
        /// The endpoints of the web server.
        /// </summary>
        [XmlElement("endpoint")]
        public List<EndpointConfig> Endpoints { get; set; }

        /// <summary>
        /// The route of the web server.
        /// </summary>
        [XmlElement("route")]
        public string Route { get; set; }

        /// <summary>
        /// Optional fine-tuning of the underlying Kestrel server, including all request limits. When
        /// the element is omitted the web server keeps its built-in defaults, so this block only ever
        /// applies values that are explicitly opted into.
        /// </summary>
        [XmlElement("kestrel")]
        public KestrelConfig Kestrel { get; set; }

        /// <summary>
        /// Optional configuration of the session and its cookie. When the element is omitted the
        /// built-in defaults apply, so this block only ever changes values explicitly opted into.
        /// </summary>
        [XmlElement("session")]
        public SessionConfig Session { get; set; }

        /// <summary>
        /// Root directory of packages.
        /// </summary>
        [XmlElement("packages")]
        public string PackageBase { get; set; }

        /// <summary>
        /// Root directory of assets.
        /// </summary>
        [XmlElement("assets")]
        public string AssetBase { get; set; }

        /// <summary>
        /// Root directory of the data.
        /// </summary>
        [XmlElement("data")]
        public string DataBase { get; set; }

        /// <summary>
        /// The uri base path of the web server.
        /// </summary>
        [XmlElement("contextpath")]
        public string ContextPath { get; set; }

        /// <summary>
        /// The culture
        /// </summary>
        [XmlElement("culture")]
        public string Culture { get; set; }

        /// <summary>
        /// The log settings.
        /// </summary>
        [XmlElement("log")]
        public SettingLogItem Log { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HttpServerConfig()
        {
        }
    }
}