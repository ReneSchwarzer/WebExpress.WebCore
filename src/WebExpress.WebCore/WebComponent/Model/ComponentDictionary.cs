using System.Collections.Generic;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebComponent.Model
{
    /// <summary>
    /// Internal management of components.
    /// key = plugin
    /// value = component item
    /// </summary>
    internal class ComponentDictionary : Dictionary<IPluginContext, IList<ComponentItem>>
    {

    }
}
