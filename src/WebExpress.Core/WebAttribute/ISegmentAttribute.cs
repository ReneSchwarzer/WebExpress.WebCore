using WebExpress.Core.WebUri;

namespace WebExpress.Core.WebAttribute
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
