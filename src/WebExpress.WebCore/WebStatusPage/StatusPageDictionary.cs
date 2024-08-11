using System.Collections.Generic;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebStatusPage
{
    /// <summary>
    /// key = plugin context
    /// value = ResponseDictionaryItem
    /// </summary>
    public class StatusPageDictionary : Dictionary<IPluginContext, StatusPageDictionaryItem>
    {
    }
}
