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
        /// The maximum allowed size of the combined request headers, in bytes.
        /// A value of <c>0</c> means the Kestrel default (32 KiB) is used.
        /// </summary>
        [XmlElement("maxrequestheaderstotalsize", DataType = "int")]
        public int MaxRequestHeadersTotalSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LimitConfig()
        {
        }
    }
}