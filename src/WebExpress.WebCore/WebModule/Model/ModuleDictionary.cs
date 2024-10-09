using System.Collections.Generic;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebModule.Model
{
    /// <summary>
    /// Key = plugin context 
    /// Value = { Key = module id, Value = module item }
    /// </summary>
    internal class ModuleDictionary : Dictionary<IPluginContext, Dictionary<string, ModuleItem>>
    {
    }
}
