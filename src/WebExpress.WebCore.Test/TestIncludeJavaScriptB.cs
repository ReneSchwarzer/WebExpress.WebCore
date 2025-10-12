using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebInclude;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy include for testing purposes.
    /// </summary>
    [Asset("/myX.js")]
    [Asset("/myY.js")]
    [Asset("/myZ.js")]
    public sealed class TestIncludeJavaScriptB : IInclude
    {

    }
}
