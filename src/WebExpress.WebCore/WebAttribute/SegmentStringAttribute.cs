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
        where TParameter : IParameterStatic, new()
    {
        /// <summary>
        /// Returns or sets the tag.
        /// </summary>
        private string Tag { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public SegmentStringAttribute(string tag = null)
        {
            Tag = tag;
        }

        /// <summary>
        /// Conversion to a path segment.
        /// </summary>
        /// <returns>The path segment.</returns>
        public IUriPathSegment ToPathSegment()
        {
            return new UriPathSegmentVariableString<TParameter>(Tag);
        }
    }
}
