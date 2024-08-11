using System;
using System.Globalization;
using WebExpress.WebCore.Internationalization;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// constant path segment.
    /// </summary>
    public class UriPathSegmentRoot : IUriPathSegment
    {
        /// <summary>
        /// Returns or sets the id.
        /// </summary>
        public string Id => "ROOT";

        /// <summary>
        /// Returns or sets the path text.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Returns or sets the display text.
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// Returns or sets the tag.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Checks for empty path segment.
        /// </summary>
        public bool IsEmpty => false;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="value">The name.</param>
        /// <param name="display">The display text.</param>
        /// <param name="tag">The tag or null</param>
        public UriPathSegmentRoot(string display = null, object tag = null)
        {
            Value = "/";
            Display = display;
            Tag = tag;
        }

        /// <summary>
        /// Checks whether the node matches the path element.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the path element matched, false otherwise.</returns>
        public bool IsMatched(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return Value.Equals(value, StringComparison.OrdinalIgnoreCase) ||
                   (Value + "/").Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Make a deep copy.
        /// </summary>
        /// <returns>The copy.</returns>
        public virtual IUriPathSegment Copy()
        {
            return new UriPathSegmentRoot(Display, Tag);
        }

        /// <summary>
        /// Compare the object.
        /// </summary>
        /// <param name="obj">The comparison object.</param>
        /// <returns>true if equals, false otherwise</returns>
        public virtual bool Equals(IUriPathSegment obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is UriPathSegmentRoot segment;
        }

        /// <summary>
        /// Returns or sets the display text.
        /// </summary>
        /// <param name="culture">The culture.</param>
        public virtual string GetDisplay(CultureInfo culture)
        {
            return InternationalizationManager.I18N(culture, Display);
        }

        /// <summary>
        /// Converts the segment to a string.
        /// </summary>
        /// <returns>A string that represents the current segment.</returns>
        public override string ToString()
        {
            return Value ?? "<null>";
        }
    }
}