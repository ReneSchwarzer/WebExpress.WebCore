using System.Collections.Generic;
using WebExpress.Core.WebEvent;
using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebJob
{
    /// <summary>
    /// key = plugin context
    /// value = ressource items
    /// </summary>
    internal class EventDictionary : Dictionary<IPluginContext, IList<IEventHandler>>
    {
    }
}
