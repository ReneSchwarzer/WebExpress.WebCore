namespace WebExpress.WebCore.WebPackage.Model
{
    /// <summary>
    /// Represents the result of a package lifecycle operation.
    /// </summary>
    public sealed class PackageOperationResult
    {
        /// <summary>
        /// Gets or sets whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets an optional operation message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the affected package, if available.
        /// </summary>
        public PackageCatalogItem Package { get; set; }

        /// <summary>
        /// Creates a successful operation result.
        /// </summary>
        /// <param name="message">The result message.</param>
        /// <param name="package">The affected package.</param>
        /// <returns>The created operation result.</returns>
        public static PackageOperationResult Ok(string message, PackageCatalogItem package = null)
        {
            return new PackageOperationResult()
            {
                Success = true,
                Message = message,
                Package = package
            };
        }

        /// <summary>
        /// Creates a failed operation result.
        /// </summary>
        /// <param name="message">The result message.</param>
        /// <param name="package">The affected package.</param>
        /// <returns>The created operation result.</returns>
        public static PackageOperationResult Failed(string message, PackageCatalogItem package = null)
        {
            return new PackageOperationResult()
            {
                Success = false,
                Message = message,
                Package = package
            };
        }
    }
}
