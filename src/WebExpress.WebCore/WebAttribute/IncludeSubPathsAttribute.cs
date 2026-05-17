using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Determines whether all resources below the specified path (including segment) are also processed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class IncludeSubPathsAttribute : Attribute, IEndpointAttribute
    {
        /// <summary>
        /// Gets a value indicating whether subpaths are included in the operation.
        /// </summary>
        public bool IncludeSubPaths { get; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="includeSubPaths">All subpaths are included.</param>
        public IncludeSubPathsAttribute(bool includeSubPaths = true)
        {
            IncludeSubPaths = includeSubPaths;
        }
    }
}
