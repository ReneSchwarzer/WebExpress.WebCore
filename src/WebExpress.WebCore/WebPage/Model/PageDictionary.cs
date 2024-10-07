using System.Collections.Generic;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebPage.Model
{
    /// <summary>
    /// key = plugin context
    /// value = { key = resource id, value = ressource item }
    /// </summary>
    internal class PageDictionary : Dictionary<IPluginContext, Dictionary<string, PageItem>>
    {
    }
}
