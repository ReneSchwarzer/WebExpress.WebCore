using System.Collections.Generic;

namespace WebExpress.Core.WebUri
{
    /// <summary>
    /// Variable path segment.
    /// </summary>
    public class UriPathSegmentVariableString : UriPathSegmentVariable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The path text.</param>
        /// <param name="tag">The tag or null</param>
        public UriPathSegmentVariableString(string name, object tag = null)
            : base(name, tag)
        {
            VariableName = name;
            Value = name;
            Display = name;
            Expression = "^[^\"]*$";
            Tag = tag;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">The path text.</param>
        /// <param name="display">The display text.</param>
        /// <param name="tag">The tag or null</param>
        public UriPathSegmentVariableString(string name, string display, object tag = null)
            : base(name, tag)
        {
            VariableName = name;
            Value = name;
            Display = display;
            Expression = "^[^\"]*$";
            Tag = tag;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="segment">The path segment to copy.</param>
        public UriPathSegmentVariableString(UriPathSegmentVariableString segment)
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
            return new UriPathSegmentVariableString(this) { Value = Value };
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