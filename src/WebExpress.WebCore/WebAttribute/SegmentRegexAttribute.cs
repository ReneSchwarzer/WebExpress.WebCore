using System;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to define a regex segment in a URI path.
    /// </summary>
    /// <typeparam name="TParameter">
    /// The type of parameter to associate with the segment key.
    /// </typeparam>
    [AttributeUsage(AttributeTargets.Class)]
    public class SegmentRegexAttribute<TParameter> : Attribute, IEndpointAttribute, ISegmentAttribute
        where TParameter : IParameterStatic, new()
    {
        /// <summary>
        /// Reurns or sets the string representation of the expression.
        /// </summary>
        private string Expression { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        private string Tag { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="expression">The regular expression.</param>
        /// <param name="tag">The tag.</param>
        public SegmentRegexAttribute(string expression, string tag = null)
        {
            Expression = expression;
            Tag = tag;
        }

        /// <summary>
        /// Conversion to a path segment.
        /// </summary>
        /// <returns>The path segment.</returns>
        public IUriPathSegment ToPathSegment()
        {
            return new UriPathSegmentVariableRegex<TParameter>(Expression, Tag);
        }
    }
}
