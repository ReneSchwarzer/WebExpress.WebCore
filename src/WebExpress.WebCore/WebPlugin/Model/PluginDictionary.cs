using System.Collections.Generic;

namespace WebExpress.WebCore.WebPlugin.Model
{
    /// <summary>
    /// Verzeichnis über die registrieten Plugins
    /// Key = PluginId
    /// Value = Plugin-Metadaten
    /// </summary>
    internal class PluginDictionary : Dictionary<string, PluginItem>
    {
    }
}
