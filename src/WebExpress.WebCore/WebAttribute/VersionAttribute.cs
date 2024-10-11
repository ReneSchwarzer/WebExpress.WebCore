using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifying a version for a rest api.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class VersionAttribute : Attribute, IRestApiAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="version">The version number of the rest api.</param>
        public VersionAttribute(uint version)
        {

        }
    }
}
