namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Determines whether all resources below the specified path (including segment) are also processed.
    /// </summary>
    public class IncludeSubPathsAttribute : System.Attribute, IResourceAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="includeSubPaths">All subpaths are included.</param>
        public IncludeSubPathsAttribute(bool includeSubPaths)
        {

        }
    }
}
