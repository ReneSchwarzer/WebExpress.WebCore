using System.Collections.Generic;
using WebExpress.WebCore.WebEvent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebJob
{
    /// <summary>
    /// key = plugin context
    /// value = ressource items
    /// </summary>
    internal class EventDictionary : Dictionary<IPluginContext, IList<IEventHandler>>
    {
    }
}
