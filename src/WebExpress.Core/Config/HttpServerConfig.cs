using System.Collections.Generic;
using System.Xml.Serialization;
using WebExpress.Core.Setting;

namespace WebExpress.Core.Config
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
        /// The uri of the web server.
        /// </summary>
        [XmlElement("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// The limitations.
        /// </summary>
        [XmlElement("limit")]
        public LimitConfig Limit { get; set; }

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
        /// Constructor
        /// </summary>
        public HttpServerConfig()
        {
        }
    }
}