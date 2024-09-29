using System.Globalization;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.Internationalization
{
    /// <summary>
    /// Provides internationalization (i18n) functionalities.
    /// </summary>
    public static class I18N
    {
        /// <summary>
        /// Translates a given key to the default language.
        /// </summary>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string Translate(string key)
        {
            return WebEx.InternationalizationManager?.Translate(key) ?? key;
        }

        /// <summary>
        /// Translates a given key to the default language.
        /// </summary>
        /// <param name="key">The internationalization key.</param>
        /// <param name="args">The formatting arguments.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string Translate(string key, params object[] args)
        {
            return WebEx.InternationalizationManager?.Translate(key, args) ?? key;
        }

        /// <summary>
        /// Translates a given key to the specified language.
        /// </summary>
        /// <param name="obj">An internationalization object that is being extended.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string Translate(II18N obj, string key)
        {
            return WebEx.InternationalizationManager?.Translate(obj, key) ?? key;
        }

        /// <summary>
        /// Translates a given key to the specified language.
        /// </summary>
        /// <param name="obj">An internationalization object that is being extended.</param>
        /// <param name="key">The internationalization key.</param>
        /// <param name="args">The formatting arguments.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string Translate(II18N obj, string key, params object[] args)
        {
            return WebEx.InternationalizationManager?.Translate(obj, key, args) ?? key;
        }

        /// <summary>
        /// Translates a given key to the specified language.
        /// </summary>
        /// <param name="request">The request with the language to use.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string Translate(Request request, string key)
        {
            return WebEx.InternationalizationManager.Translate(request, key) ?? key;
        }

        /// <summary>
        /// Translates a given key to the specified language.
        /// </summary>
        /// <param name="request">The request with the language to use.</param>
        /// <param name="key">The internationalization key.</param>
        /// <param name="args">The formatting arguments.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string Translate(Request request, string key, params object[] args)
        {
            return WebEx.InternationalizationManager?.Translate(request, key, args) ?? key;
        }

        /// <summary>
        /// Translates a given key to the specified language.
        /// </summary>
        /// <param name="culture">The culture with the language to use.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string Translate(CultureInfo culture, string key)
        {
            return WebEx.InternationalizationManager?.Translate(culture, key) ?? key;
        }

        /// <summary>
        /// Translates a given key to the specified language.
        /// </summary>
        /// <param name="culture">The culture with the language to use.</param>
        /// <param name="key">The internationalization key.</param>
        /// <param name="args">The formatting arguments.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string Translate(CultureInfo culture, string key, params object[] args)
        {
            return WebEx.InternationalizationManager?.Translate(culture, key, args) ?? key;
        }

        /// <summary>
        /// Translates a given key to the specified language.
        /// </summary>
        /// <param name="culture">The culture with the language to use.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string Translate(CultureInfo culture, string pluginId, string key)
        {
            return WebEx.InternationalizationManager?.Translate(culture, pluginId, key) ?? key;
        }

        /// <summary>
        /// Translates a given key to the specified language.
        /// </summary>
        /// <param name="culture">The culture with the language to use.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="key">The internationalization key.</param>
        /// <param name="args">The formatting arguments.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string Translate(CultureInfo culture, string pluginId, string key, params object[] args)
        {
            return WebEx.InternationalizationManager?.Translate(culture, pluginId, key, args) ?? key;
        }
    }
}
