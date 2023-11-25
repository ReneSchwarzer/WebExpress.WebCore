using System.Collections.Generic;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebComponent
{
    /// <summary>
    /// Internal management of components.
    /// key = plugin
    /// value = component item
    /// </summary>
    public class ComponentDictionary : Dictionary<IPluginContext, IList<ComponentItem>>
    {

    }
}
