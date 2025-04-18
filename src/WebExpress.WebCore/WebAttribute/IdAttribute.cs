namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// The unique identification key.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class IdAttribute : System.Attribute, IPluginAttribute, IApplicationAttribute, IEndpointAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="id">The id.</param>
        public IdAttribute(string id)
        {

        }
    }
}
