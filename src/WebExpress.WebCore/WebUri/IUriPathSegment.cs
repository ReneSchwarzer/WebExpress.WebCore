using System.Globalization;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// The path segment of a resource uri.
    /// </summary>
    public interface IUriPathSegment
    {
        /// <summary>
        /// Returns or sets the id.
        /// </summary>
        internal string Id { get; }

        /// <summary>
        /// Returns the value.
        /// </summary>
        string Value { get; }

        /// <summary>
        /// Returns or sets the display text.
        /// </summary>
        string Display { get; set; }

        /// <summary>
        /// Returns the tag.
        /// </summary>
        object Tag { get; }

        /// <summary>
        /// Checks for empty path segment.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Checks whether the node matches the path element.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the path element matched, false otherwise.</returns>
        bool IsMatched(string value);

        /// <summary>
        /// Make a deep copy.
        /// </summary>
        /// <returns>The copy.</returns>
        IUriPathSegment Copy();

        /// <summary>
        /// Compare the object.
        /// </summary>
        /// <param name="obj">The comparison object.</param>
        /// <returns>true if equals, false otherwise</returns>
        bool Equals(IUriPathSegment obj);

        /// <summary>
        /// Returns or sets the display text.
        /// </summary>
        /// <param name="culture">The culture.</param>
        string GetDisplay(CultureInfo culture);
    }
}