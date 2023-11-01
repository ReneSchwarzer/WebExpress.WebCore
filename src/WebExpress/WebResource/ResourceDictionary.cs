using System.Collections.Generic;
using WebExpress.WebPlugin;

namespace WebExpress.WebResource
{
    /// <summary>
    /// key = plugin context
    /// value = { key = resource id, value = ressource item }
    /// </summary>
    internal class ResourceDictionary : Dictionary<IPluginContext, Dictionary<string, ResourceItem>>
    {
    }
}
