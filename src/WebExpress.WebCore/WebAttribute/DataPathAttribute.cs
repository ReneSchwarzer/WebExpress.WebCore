namespace WebExpress.WebCore.WebAttribute
{
    public class DataPathAttribute : System.Attribute, IApplicationAttribute, IModuleAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="dataPath">The path for the data.</param>
        public DataPathAttribute(string dataPath)
        {

        }
    }
}
