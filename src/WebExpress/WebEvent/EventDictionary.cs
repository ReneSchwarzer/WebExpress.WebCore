using System.Collections.Generic;
using WebExpress.WebEvent;
using WebExpress.WebPlugin;

namespace WebExpress.WebJob
{
    /// <summary>
    /// key = plugin context
    /// value = ressource items
    /// </summary>
    internal class EventDictionary : Dictionary<IPluginContext, IList<IEventHandler>>
    {
    }
}
