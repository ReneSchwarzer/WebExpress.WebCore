using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Interface for converting an object to a URI path segment.
    /// </summary>
    public interface ISegmentAttribute
    {
        /// <summary>
        /// Conversion to a path segment.
        /// </summary>
        /// <returns>The path segment.</returns>
        IUriPathSegment ToPathSegment();
    }
}
