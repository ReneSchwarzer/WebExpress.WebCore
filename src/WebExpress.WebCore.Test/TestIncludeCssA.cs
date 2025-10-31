using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebInclude;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy include for testing purposes.
    /// </summary>
    [Asset("/myA.css")]
    [Asset("/myB.css")]
    [Asset("/myC.css")]
    public sealed class TestIncludeCssA : IInclude
    {

    }
}
