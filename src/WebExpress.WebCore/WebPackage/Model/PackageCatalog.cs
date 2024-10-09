using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace WebExpress.WebCore.WebPackage.Model
{
    [XmlRoot("catalog")]
    public class PackageCatalog
    {
        /// <summary>
        /// Returns the package entries in the catalog.
        /// </summary>
        [XmlElement("package")]
        public List<PackageCatalogItem> Packages { get; } = new List<PackageCatalogItem>();

        /// <summary>
        /// Returns the system package entries in the catalog.
        /// </summary>
        [XmlIgnore]
        public List<PackageCatalogItem> SystemPackages { get; } = new List<PackageCatalogItem>();

        /// <summary>
        /// Locates a specific catalog item.
        /// </summary>
        /// <param name="id">The package id.</param>
        /// <returns>The catalog item or null.</returns>
        public PackageCatalogItem Find(string id)
        {
            return Packages
                .Where(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }
    }
}
