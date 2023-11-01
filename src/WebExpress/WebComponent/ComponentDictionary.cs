using System.Collections.Generic;
using WebExpress.WebPlugin;

namespace WebExpress.WebComponent
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
