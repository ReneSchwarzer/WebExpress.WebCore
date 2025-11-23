using System;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to define a double segment in a URI path.
    /// </summary>
    /// <typeparam name="TParameter">
    /// The type of parameter to associate with the segment key.
    /// </typeparam>
    [AttributeUsage(AttributeTargets.Class)]
    public class SegmentDoubleAttribute<TParameter> : Attribute, IEndpointAttribute, ISegmentAttribute
        where TParameter : IParameter
    {
        /// <summary>
        /// Returns or sets the name of the variable.
        /// </summary>
        private string VariableName { get; set; }

        /// <summary>
        /// Returns or sets the display string.
        /// </summary>
        private string Display { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="variableName">The name of the variable.</param>
        /// <param name="display">The display string.</param>
        public SegmentDoubleAttribute(string variableName, string display)
        {
            VariableName = variableName;
            Display = display;
        }

        /// <summary>
        /// Conversion to a path segment.
        /// </summary>
        /// <returns>The path segment.</returns>
        public IUriPathSegment ToPathSegment()
        {
            return new UriPathSegmentVariableDouble<TParameter>(VariableName, Display);
        }
    }
}
