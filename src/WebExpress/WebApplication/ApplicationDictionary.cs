using System.Collections.Generic;
using WebExpress.WebPlugin;

namespace WebExpress.WebApplication
{
    /// <summary>
    /// Key = Plugin context
    /// Value = { Key = application id, Value = application item }
    /// </summary>
    internal class ApplicationDictionary : Dictionary<IPluginContext, Dictionary<string, ApplicationItem>>
    {
    }
}
