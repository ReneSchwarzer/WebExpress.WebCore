using System;

namespace WebExpress.WebCore.WebAttribute
{

    /// <summary>
    /// Attribute to specify the path for assets in the application.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AssetPathAttribute : Attribute, IApplicationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="assetPath">The path for assets.</param>
        public AssetPathAttribute(string assetPath)
        {

        }
    }
}
