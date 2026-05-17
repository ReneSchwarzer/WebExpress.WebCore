namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// The path segment of a resource uri.
    /// </summary>
    public interface IUriPathSegment
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        string Value { get; }

        /// <summary>
        /// Gets the tag.
        /// </summary>
        object Tag { get; }

        /// <summary>
        /// Checks for empty path segment.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets a value indicating whether the item is hidden.
        /// </summary>
        /// <remarks>
        /// This property can be used to determine if the item should be displayed in user
        /// interfaces or lists.
        /// </remarks>
        bool IsHidden { get; set; }

        /// <summary>
        /// Gets the URI to which the user is redirected.
        /// </summary>
        IUri Uri { get; set; }

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
    }
}