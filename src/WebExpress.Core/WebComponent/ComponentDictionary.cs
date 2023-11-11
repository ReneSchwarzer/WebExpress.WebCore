using System.Collections.Generic;
using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebComponent
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
