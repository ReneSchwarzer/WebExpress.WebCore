using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebAsset.Model
{
    /// <summary>
    /// Represents a dictionary that maps plugin contexts to application contexts and asset items.
    /// key = plugin context
    /// value = { key = resource type, value = ressource item }
    /// </summary>
    internal class AssetItemDictionary : Dictionary<IPluginContext, Dictionary<IApplicationContext, List<AssetItem>>>
    {
        /// <summary>
        /// Adds a asset item to the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="assetItem">The resource item.</param>
        /// <returns>True if the resource item was added successfully, false if an element with the same status code already exists.</returns>
        public bool AddAssetItem(IPluginContext pluginContext, IApplicationContext applicationContext, AssetItem assetItem)
        {
            var type = assetItem.AssetClass;

            if (!typeof(IAsset).IsAssignableFrom(type))
            {
                return false;
            }

            if (!ContainsKey(pluginContext))
            {
                this[pluginContext] = [];
            }

            var appContextDict = this[pluginContext];

            if (!appContextDict.ContainsKey(applicationContext))
            {
                appContextDict[applicationContext] = [];
            }

            var assetList = appContextDict[applicationContext];

            assetList.RemoveAll(x => x.AssetContext?.EndpointId == assetItem.AssetContext.EndpointId);
            assetList.Add(assetItem);

            return true;
        }

        /// <summary>
        /// Returns the asset items from the dictionary.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of asset items</returns>
        public IEnumerable<AssetItem> GetAssetItems(IApplicationContext applicationContext)
        {
            if (ContainsKey(applicationContext?.PluginContext))
            {
                var appContextDict = this[applicationContext?.PluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var assetList = appContextDict[applicationContext];

                    return assetList;
                }
            }

            return [];
        }
    }
}
