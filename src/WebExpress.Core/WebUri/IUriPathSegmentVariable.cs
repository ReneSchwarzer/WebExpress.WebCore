using System.Collections.Generic;

namespace WebExpress.Core.WebUri
{
    /// <summary>
    /// The path segment of a uri.
    /// </summary>
    public interface IUriPathSegmentVariable : IUriPathSegment
    {
        /// <summary>
        /// Returns or sets the value.
        /// </summary>
        new string Value { get; set; }

        /// <summary>
        /// Returns the variable name.
        /// </summary>
        string VariableName { get; }

        /// <summary>
        /// Returns the regex expression.
        /// </summary>
        string Expression { get; }

        /// <summary>
        /// Returns the variable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The variable value pair.</returns>
        IDictionary<string, string> GetVariable(string value);
    }
}