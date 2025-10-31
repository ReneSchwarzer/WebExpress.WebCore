using System;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPackage.Model;

namespace WebExpress.WebCore.WebPackage
{
    /// <summary>
    /// The package manager manages packages with WebExpress extensions. The packages must 
    /// be in WebExpressPackage format (*.wxp).
    /// </summary>
    public interface IPackageManager : IComponentManager
    {
        /// <summary>
        /// An event that fires when an package is added.
        /// </summary>
        event EventHandler<PackageCatalogItem> AddPackage;

        /// <summary>
        /// An event that fires when an package is removed.
        /// </summary>
        event EventHandler<PackageCatalogItem> RemovePackage;

        /// <summary>
        /// Returns the catalog of installed packages.
        /// </summary>
        PackageCatalog Catalog { get; }
    }
}
