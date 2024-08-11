using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Marks a plugin as dependent on another plugin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependencyAttribute : System.Attribute, IPluginAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="dependency">The Id of the plugin to which there is a dependency.</param>
        public DependencyAttribute(string dependency)
        {

        }
    }
}
