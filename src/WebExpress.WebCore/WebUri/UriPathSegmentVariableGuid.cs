using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebParameter;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// Represents a URI path segment variable for GUIDs.
    /// </summary>
    /// <typeparam name="TParameter">The parameter type.</typeparam>
    public class UriPathSegmentVariableGuid<TParameter> : UriPathSegmentVariable<TParameter>
        where TParameter : IParameter
    {
        /// <summary>
        /// The display formats of the guid.
        /// </summary>
        public enum Format
        {
            /// <summary>
            /// Full format of the guid.
            /// </summary>
            Full,

            /// <summary>
            /// Simple format of the guid.
            /// </summary>
            Simple
        }

        /// <summary>
        /// Returns the display format.
        /// </summary>
        public Format DisplayFormat { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The path text.</param>
        /// <param name="tag">The tag or null</param>
        public UriPathSegmentVariableGuid(string name, object tag = null)
            : this(name, Format.Simple, tag)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayFormat">The display format.</param>
        /// <param name="tag">The tag or null</param>
        public UriPathSegmentVariableGuid(string name, Format displayFormat, object tag = null)
            : base(name, tag)
        {
            VariableName = name;
            DisplayFormat = displayFormat;
            Expression = @"^(\{){0,1}(([0-9a-fA-F]{8})\-([0-9a-fA-F]{4})\-([0-9a-fA-F]{4})\-([0-9a-fA-F]{4})\-([0-9a-fA-F]{12}))(\}){0,1}$";
        }

        /// <summary>
        /// Returns the variable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The variable value pair.</returns>
        public override IDictionary<string, string> GetVariable(string value)
        {
            var match = Regex.Match(value, Expression, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (match.Success)
            {
                var dict = new Dictionary<string, string>
                    {
                        { VariableName, match.Groups[2].ToString() }
                    };

                return dict;
            }

            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Make a deep copy.
        /// </summary>
        /// <returns>The copy.</returns>
        public override IUriPathSegment Copy()
        {
            return new UriPathSegmentVariableGuid<TParameter>(VariableName, DisplayFormat)
            {
                Expression = Expression,
                Value = Value
            };
        }

        /// <summary>
        /// Returns a string that represents the display text for the current instance.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        /// <returns>
        /// A string containing the display text associated with the instance. The 
        /// value may be empty if no display text is available.
        /// </returns>
        public override string GetDisplayText(IRenderContext renderContext)
        {
            var match = Regex.Match(Value, Expression, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var guid = DisplayFormat == Format.Simple ? match.Groups[7].ToString() : match.Groups[2].ToString();

            if (string.IsNullOrWhiteSpace(Value) || !Value.Contains("{0}"))
            {
                return guid;
            }

            return string.Format
            (
                I18N.Translate(renderContext, Value),
                guid
            );
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