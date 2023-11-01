using System.Collections.Generic;
using WebExpress.WebPlugin;

namespace WebExpress.WebJob
{
    /// <summary>
    /// key = plugin context
    /// value = ressource items
    /// </summary>
    internal class ScheduleDictionary : Dictionary<IPluginContext, IList<ScheduleStaticItem>>
    {
    }
}
