using System.Collections.Generic;
using WebExpress.WebPlugin;

namespace WebExpress.WebModule
{
    /// <summary>
    /// Key = plugin context 
    /// Value = { Key = module id, Value = module item }
    /// </summary>
    internal class ModuleDictionary : Dictionary<IPluginContext, Dictionary<string, ModuleItem>>
    {
    }
}
