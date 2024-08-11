using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using WebExpress.WebCore.Internationalization;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// Variable path segment.
    /// </summary>
    public class UriPathSegmentVariableGuid : UriPathSegmentVariable
    {
        /// <summary>
        /// The display formats of the guid.
        /// </summary>
        public enum Format { Full, Simple }

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
            : this(name, null, tag)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="value">The path text.</param>
        /// <param name="display">The display text.</param>
        /// <param name="tag">The tag or null</param>
        public UriPathSegmentVariableGuid(string name, string display, object tag = null)
            : this(name, display, Format.Full, tag)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="value">The path text.</param>
        /// <param name="display">The display text.</param>
        /// <param name="displayFormat">The display format.</param>
        /// <param name="tag">The tag or null</param>
        public UriPathSegmentVariableGuid(string name, string display, Format displayFormat, object tag = null)
            : base(name, display, tag)
        {
            VariableName = name;
            DisplayFormat = displayFormat;
            Expression = @"^(\{){0,1}(([0-9a-fA-F]{8})\-([0-9a-fA-F]{4})\-([0-9a-fA-F]{4})\-([0-9a-fA-F]{4})\-([0-9a-fA-F]{12}))(\}){0,1}$";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="segment">The path segment to copy.</param>
        public UriPathSegmentVariableGuid(UriPathSegmentVariableGuid segment)
            : base(segment.VariableName, segment.Display, segment.Tag)
        {
            DisplayFormat = segment.DisplayFormat;
            Expression = segment.Expression;
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
            return new UriPathSegmentVariableGuid(this) { Value = Value };
        }

        /// <summary>
        /// Returns or sets the display text.
        /// </summary>
        /// <param name="culture">The culture.</param>
        public override string GetDisplay(CultureInfo culture)
        {
            var match = Regex.Match(Value, Expression, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var guid = DisplayFormat == Format.Simple ? match.Groups[7].ToString() : match.Groups[2].ToString();

            if (string.IsNullOrWhiteSpace(Display) || !Display.Contains("{0}"))
            {
                return guid;
            }

            return string.Format
            (
                InternationalizationManager.I18N(culture, Display),
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