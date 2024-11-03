using System;
using System.Globalization;
using WebExpress.WebCore.Internationalization;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// Represents the root segment of a URI path.
    /// </summary>
    public class UriPathSegmentRoot : IUriPathSegment
    {
        /// <summary>
        /// Returns the ID of the segment.
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
        /// Returns a value indicating whether the path segment is empty.
        /// </summary>
        public bool IsEmpty => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriPathSegmentRoot"/> class.
        /// </summary>
        /// <param name="display">The display text.</param>
        /// <param name="tag">The tag or null.</param>
        public UriPathSegmentRoot(string display = null, object tag = null)
        {
            Value = "/";
            Display = display;
            Tag = tag;
        }

        /// <summary>
        /// Checks whether the node matches the specified path element.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the path element matches, false otherwise.</returns>
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
        /// Creates a deep copy of the current segment.
        /// </summary>
        /// <returns>A copy of the current segment.</returns>
        public virtual IUriPathSegment Copy()
        {
            return new UriPathSegmentRoot(Display, Tag);
        }

        /// <summary>
        /// Compares the current segment with another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if the objects are equal, false otherwise.</returns>
        public virtual bool Equals(IUriPathSegment obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is UriPathSegmentRoot;
        }

        /// <summary>
        /// Returns the display text for the specified culture.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <returns>The display text for the specified culture.</returns>
        public virtual string GetDisplay(CultureInfo culture)
        {
            return I18N.Translate(culture, Display);
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