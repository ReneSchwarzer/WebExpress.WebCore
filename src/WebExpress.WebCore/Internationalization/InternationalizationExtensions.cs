using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.Internationalization
{
    public static class InternationalizationExtensions
    {
        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="obj">An internationalization object that is being extended.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(this II18N obj, string key)
        {
            return WebEx.ComponentHub?.InternationalizationManager.Translate(obj, key);
        }

        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="obj">An internationalization object that is being extended.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(this II18N obj, string pluginId, string key)
        {
            return WebEx.ComponentHub?.InternationalizationManager.Translate(obj.Culture, pluginId, key);
        }

        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="obj">An internationalization object that is being extended.</param>
        /// <param name="applicationContext">The allication context.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(this II18N obj, IApplicationContext applicationContext, string key)
        {
            return WebEx.ComponentHub?.InternationalizationManager.Translate(obj.Culture, applicationContext?.PluginContext?.PluginId, key);
        }

        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="obj">An render context object that is being extended.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(this RenderContext obj, string pluginId, string key)
        {
            return WebEx.ComponentHub?.InternationalizationManager.Translate(obj.Culture, pluginId, key);
        }

        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="obj">An render context object that is being extended.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(this RenderContext obj, string key)
        {
            return WebEx.ComponentHub?.InternationalizationManager.Translate(obj.Culture, obj?.ApplicationContext?.PluginContext?.PluginId, key);
        }

        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="obj">An render context object that is being extended.</param>
        /// <param name="applicationContext">The allication context.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(this RenderContext obj, IApplicationContext applicationContext, string key)
        {
            return WebEx.ComponentHub?.InternationalizationManager.Translate(obj.Culture, applicationContext?.PluginContext?.PluginId, key);
        }
    }
}
