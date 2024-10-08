namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// The unique identification key.
    /// </summary>
    public class IdAttribute : System.Attribute, IPluginAttribute, IApplicationAttribute, IModuleAttribute, IEndpointAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The id.</param>
        public IdAttribute(string id)
        {

        }
    }
}
