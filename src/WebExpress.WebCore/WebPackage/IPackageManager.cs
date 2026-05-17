using System;
using System.IO;
using System.Collections.Generic;
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
        /// Gets the catalog of installed packages.
        /// </summary>
        PackageCatalog Catalog { get; }

        /// <summary>
        /// Returns all package entries from the package catalog.
        /// </summary>
        /// <returns>An enumerable collection with all package entries.</returns>
        IEnumerable<PackageCatalogItem> GetPackages();

        /// <summary>
        /// Returns a package by id.
        /// </summary>
        /// <param name="packageId">The package id.</param>
        /// <returns>The package or null.</returns>
        PackageCatalogItem GetPackage(string packageId);

        /// <summary>
        /// Validates a package file.
        /// </summary>
        /// <param name="packageFile">The package file path.</param>
        /// <param name="maxPackageBytes">Optional max allowed package size in bytes. 0 disables the limit check.</param>
        /// <param name="expectedSha256">Optional expected SHA-256 hash in hex format.</param>
        /// <returns>The validation result.</returns>
        PackageValidationResult ValidatePackage(string packageFile, long maxPackageBytes = 0, string expectedSha256 = null);

        /// <summary>
        /// Uploads and installs a package from a stream.
        /// </summary>
        /// <param name="packageStream">The package stream.</param>
        /// <param name="fileName">The package file name.</param>
        /// <param name="activate">True to activate directly after install; false to keep it disabled.</param>
        /// <param name="maxPackageBytes">Optional max allowed package size in bytes. 0 disables the limit check.</param>
        /// <param name="expectedSha256">Optional expected SHA-256 hash in hex format.</param>
        /// <returns>The operation result.</returns>
        PackageOperationResult UploadPackage(Stream packageStream, string fileName, bool activate = true, long maxPackageBytes = 0, string expectedSha256 = null);

        /// <summary>
        /// Installs a package from a file.
        /// </summary>
        /// <param name="packageFile">The package file path.</param>
        /// <param name="activate">True to activate directly after install; false to keep it disabled.</param>
        /// <param name="maxPackageBytes">Optional max allowed package size in bytes. 0 disables the limit check.</param>
        /// <param name="expectedSha256">Optional expected SHA-256 hash in hex format.</param>
        /// <returns>The operation result.</returns>
        PackageOperationResult InstallPackage(string packageFile, bool activate = true, long maxPackageBytes = 0, string expectedSha256 = null);

        /// <summary>
        /// Activates a package.
        /// </summary>
        /// <param name="packageId">The package id.</param>
        /// <returns>The operation result.</returns>
        PackageOperationResult ActivatePackage(string packageId);

        /// <summary>
        /// Deactivates a package.
        /// </summary>
        /// <param name="packageId">The package id.</param>
        /// <returns>The operation result.</returns>
        PackageOperationResult DeactivatePackage(string packageId);

        /// <summary>
        /// Updates a package from a file path.
        /// </summary>
        /// <param name="packageId">The package id.</param>
        /// <param name="packageFile">The package file path.</param>
        /// <param name="activate">True to activate directly after update; false to keep it disabled.</param>
        /// <param name="maxPackageBytes">Optional max allowed package size in bytes. 0 disables the limit check.</param>
        /// <param name="expectedSha256">Optional expected SHA-256 hash in hex format.</param>
        /// <returns>The operation result.</returns>
        PackageOperationResult UpdatePackage(string packageId, string packageFile, bool activate = true, long maxPackageBytes = 0, string expectedSha256 = null);

        /// <summary>
        /// Uninstalls and removes a package.
        /// </summary>
        /// <param name="packageId">The package id.</param>
        /// <returns>The operation result.</returns>
        PackageOperationResult UninstallPackage(string packageId);
    }
}
