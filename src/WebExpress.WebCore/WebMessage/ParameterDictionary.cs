using System.Collections.Concurrent;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Management of parameters.
    /// </summary>
    public class ParameterDictionary : ConcurrentDictionary<string, Parameter>
    {
    }
}
