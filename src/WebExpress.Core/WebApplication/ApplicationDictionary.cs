using System.Collections.Generic;
using WebExpress.Core.WebPlugin;

namespace WebExpress.Core.WebApplication
{
    /// <summary>
    /// Key = Plugin context
    /// Value = { Key = application id, Value = application item }
    /// </summary>
    internal class ApplicationDictionary : Dictionary<IPluginContext, Dictionary<string, ApplicationItem>>
    {
    }
}
