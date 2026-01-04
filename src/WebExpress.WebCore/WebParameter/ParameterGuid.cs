using System;

namespace WebExpress.WebCore.WebParameter
{
    /// <summary>
    /// Represents a guid parameter.
    /// </summary>
    public class ParameterGuid : Parameter
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ParameterGuid()
            : base("Id", null, ParameterScope.Url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class with a specified value.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        public ParameterGuid(string value)
            : base("Id", value, ParameterScope.Url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class with a specified value.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        public ParameterGuid(Guid value)
            : base("Id", value.ToString(), ParameterScope.Url)
        {
        }
    }
}
