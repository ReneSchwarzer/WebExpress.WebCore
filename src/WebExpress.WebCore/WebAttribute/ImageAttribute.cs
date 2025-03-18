using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to specify an image for a theme.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ImageAttribute : Attribute, IThemeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="image">The image.</param>
        public ImageAttribute(string image)
        {

        }
    }
}
