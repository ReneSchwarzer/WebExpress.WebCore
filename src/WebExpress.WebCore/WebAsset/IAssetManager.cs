using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebAsset
{
    /// <summary>
    /// The asset manager manages resources elements, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public interface IAssetManager : IComponentManager
    {
        /// <summary>
        /// An event that fires when an asset is added.
        /// </summary>
        event EventHandler<IAssetContext> AddAsset;

        /// <summary>
        /// An event that fires when an asset is removed.
        /// </summary>
        event EventHandler<IAssetContext> RemoveAsset;

        /// <summary>
        /// Gets all asset contexts.
        /// </summary>
        IEnumerable<IAssetContext> Assets { get; }

        /// <summary>
        /// Returns an enumeration of all containing asset contexts of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose resources are to be registered.</param>
        /// <returns>An enumeration of resource contexts.</returns>
        IEnumerable<IAssetContext> GetAssets(IPluginContext pluginContext);

        /// <summary>
        /// Returns an enumeration of asset contextes.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of asset contextes.</returns>
        IEnumerable<IAssetContext> GetAssets(IApplicationContext applicationContext);
    }
}
