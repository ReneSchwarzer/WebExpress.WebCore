namespace WebExpress.Core.WebAttribute
{
    public class DataPathAttribute : System.Attribute, IApplicationAttribute, IModuleAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataPath">The path for the data.</param>
        public DataPathAttribute(string dataPath)
        {

        }
    }
}
