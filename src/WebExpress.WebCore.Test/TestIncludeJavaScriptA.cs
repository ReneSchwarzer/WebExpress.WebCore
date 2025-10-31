using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebInclude;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy include for testing purposes.
    /// </summary>
    [Asset("/myA.js")]
    [Asset("/myB.js")]
    [Asset("/myC.js")]
    public sealed class TestIncludeJavaScriptA : IInclude
    {

    }
}
