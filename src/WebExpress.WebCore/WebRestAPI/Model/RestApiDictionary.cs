﻿using System.Collections.Generic;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebRestApi.Model
{
    /// <summary>
    /// key = plugin context
    /// value = { key = resource id, value = ressource item }
    /// </summary>
    internal class RestApiDictionary : Dictionary<IPluginContext, Dictionary<string, RestApiItem>>
    {
    }
}