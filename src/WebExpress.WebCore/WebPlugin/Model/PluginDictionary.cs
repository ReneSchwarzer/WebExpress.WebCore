using System.Collections.Generic;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebPlugin.Model
{
    /// <summary>
    /// Verzeichnis über die registrieten Plugins
    /// Key = PluginId
    /// Value = Plugin-Metadaten
    /// </summary>
    internal class PluginDictionary : Dictionary<IComponentId, PluginItem>
    {
    }
}
