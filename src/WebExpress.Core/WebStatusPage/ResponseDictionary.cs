using System.Collections.Generic;
using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebStatusPage
{
    /// <summary>
    /// key = plugin context
    /// value = ResponseDictionaryItem
    /// </summary>
    public class ResponseDictionary : Dictionary<IPluginContext, ResponseDictionaryItem>
    {
    }
}
