using System.Collections.Generic;
using System.Xml.Serialization;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebPackage.Model
{
    [XmlRoot("package")]
    public class PackageCatalogItem
    {
        /// <summary>
        /// Returns or sets Returns or sets the id.
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Returns or sets the filename.
        /// </summary>
        [XmlAttribute("file")]
        public string File { get; set; }

        /// <summary>
        /// Returns or sets the state.
        /// </summary>
        [XmlAttribute("state")]
        public PackageCatalogeItemState State { get; set; }

        /// <summary>
        /// Returns the plugins belonging to the package.
        /// </summary>
        [XmlIgnore]
        public List<IPluginContext> Plugins { get; internal set; } = new List<IPluginContext>();

        /// <summary>
        /// Returns the meta information about the package.
        /// </summary>
        [XmlIgnore]
        public PackageItem Metadata { get; set; }

        /// <summary>
        /// Conversion into a string representation of the object.
        /// </summary>
        /// <returns>The object as a string.</returns>
        public override string ToString()
        {
            return Id;
        }
    }
}
