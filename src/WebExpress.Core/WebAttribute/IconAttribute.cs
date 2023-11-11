namespace WebExpress.Core.WebAttribute
{
    public class IconAttribute : System.Attribute, IPluginAttribute, IApplicationAttribute, IModuleAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="icon">The icon.</param>
        public IconAttribute(string icon)
        {

        }
    }
}
