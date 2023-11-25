using System;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebAttribute
{
    public class SegmentIntAttribute : Attribute, IResourceAttribute, ISegmentAttribute
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
        /// Constructor
        /// </summary>
        /// <param name="variableName">The name of the variable.</param>
        /// <param name="display">The display string.</param>
        public SegmentIntAttribute(string variableName, string display)
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
            return new UriPathSegmentVariableInt(VariableName, Display);
        }
    }
}
