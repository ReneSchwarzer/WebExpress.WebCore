using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies a asset (JavaScript or StyleSheet) file to be included. This attribute can be applied multiple times to include
    /// multiple asset files.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AssetAttribute : Attribute, IIncludeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class with the specified asset file.
        /// </summary>
        /// <param name="file">The path to the asset file associated with this attribute. Cannot be null or empty.</param>
        public AssetAttribute(string file)
        {

        }
    }
}
