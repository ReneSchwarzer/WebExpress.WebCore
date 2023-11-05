using System.Collections.Generic;
using WebExpress.WebPlugin;

namespace WebExpress.WebStatusPage
{
    /// <summary>
    /// key = plugin context
    /// value = ResponseDictionaryItem
    /// </summary>
    public class ResponseDictionary : Dictionary<IPluginContext, ResponseDictionaryItem>
    {
    }
}
