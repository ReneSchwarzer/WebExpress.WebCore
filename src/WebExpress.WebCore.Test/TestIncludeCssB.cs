using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebInclude;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy include for testing purposes.
    /// </summary>
    [Asset("/myX.css")]
    [Asset("/myY.css")]
    [Asset("/myZ.css")]
    public sealed class TestIncludeCssB : IInclude
    {

    }
}
