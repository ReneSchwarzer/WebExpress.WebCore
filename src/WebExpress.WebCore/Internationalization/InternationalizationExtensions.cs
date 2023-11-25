using WebExpress.WebCore.WebApplication;

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
            return InternationalizationManager.I18N(obj, key);
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
            return InternationalizationManager.I18N(obj.Culture, pluginId, key);
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
            return InternationalizationManager.I18N(obj.Culture, applicationContext?.PluginContext?.PluginId, key);
        }
    }
}
