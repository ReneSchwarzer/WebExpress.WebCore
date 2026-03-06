using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Indicates that a segment is hidden.
    /// </summary>
    /// <remarks>
    /// This attribute can be used to determine if the segment should not be displayed in user
    /// interfaces.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SegmentHiddenAttribute : Attribute
    {

    }
}
