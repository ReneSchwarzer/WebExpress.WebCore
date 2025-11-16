using System.Collections.Generic;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// Variable path segment.
    /// </summary>
    public class UriPathSegmentVariableRegex : UriPathSegmentVariable
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="regex">The regular expression.</param>
        /// <param name="tag">The tag or null</param>
        public UriPathSegmentVariableRegex(string name, string regex, object tag = null)
            : base(name, tag)
        {
            VariableName = name;
            Value = name;
            Display = name;
            Expression = regex;
            Tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="regex">The regular expression.</param>
        /// <param name="display">The display text.</param>
        /// <param name="tag">The tag or null</param>
        public UriPathSegmentVariableRegex(string name, string regex, string display, object tag = null)
            : base(name, tag)
        {
            VariableName = name;
            Value = name;
            Display = display;
            Expression = regex;
            Tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="segment">The path segment to copy.</param>
        public UriPathSegmentVariableRegex(UriPathSegmentVariableRegex segment)
            : base(segment.VariableName, segment.Display, segment.Tag)
        {
            Expression = segment.Expression;
        }

        /// <summary>
        /// Returns the variable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The variable value pair.</returns>
        public override IDictionary<string, string> GetVariable(string value)
        {
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Make a deep copy.
        /// </summary>
        /// <returns>The copy.</returns>
        public override IUriPathSegment Copy()
        {
            return new UriPathSegmentVariableRegex(this) { Value = Value };
        }

        /// <summary>
        /// Converts the segment to a string.
        /// </summary>
        /// <returns>A string that represents the current segment.</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}