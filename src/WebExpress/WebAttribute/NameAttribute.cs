namespace WebExpress.WebAttribute
{
    public class NameAttribute : System.Attribute, IPluginAttribute, IApplicationAttribute, IModuleAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Der Name</param>
        public NameAttribute(string name)
        {

        }
    }
}
