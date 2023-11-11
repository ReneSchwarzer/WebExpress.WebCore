using System.Collections.Generic;
using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebJob
{
    /// <summary>
    /// key = plugin context
    /// value = ressource items
    /// </summary>
    internal class ScheduleDictionary : Dictionary<IPluginContext, IList<ScheduleStaticItem>>
    {
    }
}
