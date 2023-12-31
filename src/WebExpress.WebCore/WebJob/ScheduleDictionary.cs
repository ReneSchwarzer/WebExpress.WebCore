﻿using System.Collections.Generic;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebJob
{
    /// <summary>
    /// key = plugin context
    /// value = ressource items
    /// </summary>
    internal class ScheduleDictionary : Dictionary<IPluginContext, IList<ScheduleStaticItem>>
    {
    }
}
