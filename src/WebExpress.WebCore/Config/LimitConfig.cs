using System.Xml.Serialization;

namespace WebExpress.WebCore.Config
{
    /// <summary>
    /// Class for reading the limitations.
    /// </summary>
    [XmlRoot("limit", IsNullable = false)]
    public sealed class LimitConfig
    {
        /// <summary>
        /// The connection limit.
        /// </summary>
        [XmlElement("connectionlimit", DataType = "int")]
        public int ConnectionLimit { get; set; }

        /// <summary>
        /// The upload limit, in bytes.
        /// </summary>
        [XmlElement("uploadlimit", DataType = "long")]
        public long UploadLimit { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public LimitConfig()
        {
        }
    }
}