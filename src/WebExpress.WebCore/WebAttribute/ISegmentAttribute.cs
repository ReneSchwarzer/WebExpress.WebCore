using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebAttribute
{
    public interface ISegmentAttribute
    {
        /// <summary>
        /// Conversion to a path segment.
        /// </summary>
        /// <returns>The path segment.</returns>
        IUriPathSegment ToPathSegment();
    }
}
