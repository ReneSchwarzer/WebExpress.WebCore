namespace WebExpress.WebCore.WebPackage.Model
{
    /// <summary>
    /// Represents the state of a package in the catalog.
    /// </summary>
    public enum PackageCatalogeItemState
    {
        /// <summary>
        /// The package is available but has not yet been loaded by WebExpress.
        /// </summary>
        Available,

        /// <summary>
        /// The package has been loaded and is ready for use.
        /// </summary>
        Active,

        /// <summary>
        /// The package has been disabled. The use of the package is not possible.
        /// </summary>
        Disable
    }
}
