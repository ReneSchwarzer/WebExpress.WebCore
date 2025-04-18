using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;

namespace WebExpress.WebCore.WebAsset.Model
{
    /// <summary>
    /// Represents a dictionary that maps plugin contexts to application contexts and assets.
    /// key = plugin context
    /// value = { key = application context, value = asset }
    /// </summary>
    internal class AssetEndpointDictionary : Dictionary<IApplicationContext, (IAssetContext, IAsset)>
    {

    }
}
