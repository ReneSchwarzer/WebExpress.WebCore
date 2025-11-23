using System;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to define a segment string in a URI path.
    /// </summary>
    /// <typeparam name="TParameter">
    /// The type of parameter to associate with the segment key.
    /// </typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SegmentStringAttribute<TParameter> : Attribute, IEndpointAttribute, ISegmentAttribute
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
        /// <param name="display">The display string.</param>
        public SegmentStringAttribute(string display)
        {
            VariableName = (Activator.CreateInstance<TParameter>() as Parameter)?.Key?.ToLower();
            Display = display;
        }

        /// <summary>
        /// Conversion to a path segment.
        /// </summary>
        /// <returns>The path segment.</returns>
        public IUriPathSegment ToPathSegment()
        {
            return new UriPathSegmentVariableString<TParameter>(VariableName, Display);
        }
    }
}
