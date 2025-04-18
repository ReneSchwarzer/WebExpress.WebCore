using System.Xml.Serialization;

namespace WebExpress.WebCore.Setting
{
    /// <summary>
    /// Class for managing log settings.
    /// </summary>
    [XmlType("log")]
    public class SettingLogItem : ISettingItem
    {
        /// <summary>
        /// The log mode.
        /// </summary>
        [XmlAttribute(AttributeName = "modus")]
        public string Modus { get; set; }

        /// <summary>
        /// Determines whether to display debug output.
        /// </summary>
        [XmlAttribute(AttributeName = "debug")]
        public bool Debug { get; set; } = false;

        /// <summary>
        /// The directory where the log is created.
        /// </summary>
        [XmlAttribute(AttributeName = "path")]
        public string Path { get; set; }

        /// <summary>
        /// The encoding settings.
        /// </summary>
        [XmlAttribute(AttributeName = "encoding")]
        public string Encoding { get; set; }

        /// <summary>
        /// The file name of the log.
        /// </summary>
        [XmlAttribute(AttributeName = "filename")]
        public string Filename { get; set; }

        /// <summary>
        /// The format of the timestamp.
        /// </summary>
        [XmlAttribute(AttributeName = "timepattern")]
        public string Timepattern { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SettingLogItem()
        {
        }
    }
}
