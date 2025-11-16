using System;
using System.Linq.Expressions;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute to define a regex segment in a URI path.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SegmentRegexAttribute<TParameter> : Attribute, IEndpointAttribute, ISegmentAttribute
        where TParameter : Parameter
    {
        /// <summary>
        /// Returns or sets the name of the variable.
        /// </summary>
        private string VariableName { get; set; }

        /// <summary>
        /// Reurns or sets the string representation of the expression.
        /// </summary>
        private string Expression{ get; set; }

        /// <summary>
        /// Returns or sets the display string.
        /// </summary>
        private string Display { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="expression">The regular expression.</param>
        /// <param name="display">The display string.</param>
        public SegmentRegexAttribute(string expression, string display)
        {
            VariableName = (Activator.CreateInstance<TParameter>() as Parameter)?.Key?.ToLower();
            Expression = expression;
            Display = display;
        }

        /// <summary>
        /// Conversion to a path segment.
        /// </summary>
        /// <returns>The path segment.</returns>
        public IUriPathSegment ToPathSegment()
        {
            return new UriPathSegmentVariableRegex(VariableName, Expression, Display);
        }
    }
}
