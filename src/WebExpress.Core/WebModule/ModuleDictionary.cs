using System.Collections.Generic;
using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebModule
{
    /// <summary>
    /// Key = plugin context 
    /// Value = { Key = module id, Value = module item }
    /// </summary>
    internal class ModuleDictionary : Dictionary<IPluginContext, Dictionary<string, ModuleItem>>
    {
    }
}
