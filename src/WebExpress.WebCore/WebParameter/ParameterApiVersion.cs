using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebParameter
{
    /// <summary>
    /// Represents a api version parameter with a key, value, and scope.
    /// </summary>
    public class ParameterApiVersion : Parameter
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ParameterApiVersion()
            : base("_apiVersion", null, ParameterScope.Url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class with a specified value.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        public ParameterApiVersion(string value)
            : base("_apiVersion", value, ParameterScope.Url)
        {
        }

        /// <summary>
        /// Returns a string that represents the display text for the current instance.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        /// <returns>
        /// A string containing the display text associated with the instance. The 
        /// value may be empty if no display text is available.
        /// </returns>
        public override string GetDisplayText(IRenderContext renderContext)
        {
            return Value;
        }
    }
}
