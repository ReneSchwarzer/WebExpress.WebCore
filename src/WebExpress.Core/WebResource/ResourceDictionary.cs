using System.Collections.Generic;
using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebResource
{
    /// <summary>
    /// key = plugin context
    /// value = { key = resource id, value = ressource item }
    /// </summary>
    internal class ResourceDictionary : Dictionary<IPluginContext, Dictionary<string, ResourceItem>>
    {
    }
}
