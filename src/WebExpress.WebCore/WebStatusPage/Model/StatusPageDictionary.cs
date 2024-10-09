using System.Collections.Generic;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebStatusPage.Model
{
    /// <summary>
    /// key = plugin context
    /// value = ResponseDictionaryItem
    /// </summary>
    internal class StatusPageDictionary : Dictionary<IPluginContext, StatusPageDictionaryItem>
    {
    }
}
