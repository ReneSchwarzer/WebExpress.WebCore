using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to specify the data path for an application.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DataPathAttribute : System.Attribute, IApplicationAttribute
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
