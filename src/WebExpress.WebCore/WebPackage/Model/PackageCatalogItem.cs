using System.Collections.Generic;
using System.Xml.Serialization;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebPackage.Model
{
    /// <summary>
    /// Represents an item in the package catalog.
    /// </summary>
    [XmlRoot("package")]
    public class PackageCatalogItem
    {
        /// <summary>
        /// Gets or sets Returns or sets the id.
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        [XmlAttribute("file")]
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        [XmlAttribute("state")]
        public PackageCatalogeItemState State { get; set; }

        /// <summary>
        /// Gets the plugins belonging to the package.
        /// </summary>
        [XmlIgnore]
        public List<IPluginContext> Plugins { get; internal set; } = [];

        /// <summary>
        /// Gets or sets the meta information about the package.
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
