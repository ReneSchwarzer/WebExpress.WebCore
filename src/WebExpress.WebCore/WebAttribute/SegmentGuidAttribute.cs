using System;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// A dynamic path segment of type guid.
    /// </summary>
    /// <typeparam name="TParameter">
    /// The type of parameter to associate with the segment key.
    /// </typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SegmentGuidAttribute<TParameter> : Attribute, IEndpointAttribute, ISegmentAttribute
        where TParameter : IParameterStatic, new()
    {
        /// <summary>
        /// Returns or sets the display format.
        /// </summary>
        private UriPathSegmentVariableGuid<TParameter>.Format DisplayFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="displayFormat">The display format.</param>
        public SegmentGuidAttribute(UriPathSegmentVariableGuid<TParameter>.Format displayFormat = UriPathSegmentVariableGuid<TParameter>.Format.Simple)
        {
            DisplayFormat = displayFormat;
        }

        /// <summary>
        /// Conversion to a path segment.
        /// </summary>
        /// <returns>The path segment.</returns>
        public IUriPathSegment ToPathSegment()
        {
            return new UriPathSegmentVariableGuid<TParameter>(DisplayFormat);
        }
    }
}
