namespace WebExpress.WebAttribute
{
    public class AssetPathAttribute : System.Attribute, IApplicationAttribute, IModuleAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assetPath">The path for assets.</param>
        public AssetPathAttribute(string assetPath)
        {

        }
    }
}
