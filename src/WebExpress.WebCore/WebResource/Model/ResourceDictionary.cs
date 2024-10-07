using System.Collections.Generic;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebResource.Model
{
    /// <summary>
    /// key = plugin context
    /// value = { key = resource id, value = ressource item }
    /// </summary>
    internal class ResourceDictionary : Dictionary<IPluginContext, Dictionary<string, ResourceItem>>
    {
    }
}
