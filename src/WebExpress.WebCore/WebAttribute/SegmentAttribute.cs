using System;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// A static path segment.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SegmentAttribute : Attribute, IEndpointAttribute, ISegmentAttribute
    {
        /// <summary>
        /// Gets or set the segment of the uri path.
        /// </summary>
        private string Segment { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="segment">The segment of the uri path.</param>
        public SegmentAttribute(string segment)
        {
            Segment = segment;
        }

        /// <summary>
        /// Conversion to a path segment.
        /// </summary>
        /// <returns>The path segment.</returns>
        public IUriPathSegment ToPathSegment()
        {
            return new UriPathSegmentConstant(Segment) { };
        }
    }
}
