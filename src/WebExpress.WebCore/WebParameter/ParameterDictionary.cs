using System.Collections.Concurrent;

namespace WebExpress.WebCore.WebParameter
{
    /// <summary>
    /// Management of parameters.
    /// </summary>
    /// Key: parameter name
    /// Value: parameter
    public class ParameterDictionary : ConcurrentDictionary<string, IParameter>
    {
    }
}
