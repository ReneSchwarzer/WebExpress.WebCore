using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.Internationalization
{
    /// <summary>
    /// Internationalization
    /// </summary>
    public sealed class InternationalizationManager : IComponentPlugin, ISystemComponent
    {
        /// <summary>
        /// Returns the default language.
        /// </summary>
        public static CultureInfo DefaultCulture { get; private set; } = CultureInfo.CurrentCulture;

        /// <summary>
        /// Returns the directory by listing the internationalization key-value pairs.
        /// </summary>
        private static InternationalizationDictionary Dictionary { get; } = new InternationalizationDictionary();

        /// <summary>
        /// Returns or sets the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        internal InternationalizationManager()
        {
            ComponentManager.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            ComponentManager.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The reference to the context of the host.</param>
        public void Initialization(IHttpServerContext context)
        {
            HttpServerContext = context;
            DefaultCulture = HttpServerContext.Culture;

            HttpServerContext.Log.Debug
            (
                I18N("webexpress:internationalizationmanager.initialization")
            );
        }

        /// <summary>
        /// Discovers and registers entries from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose elements are to be registered.</param>
        public void Register(IPluginContext pluginContext)
        {
            var pluginId = pluginContext.PluginId;
            Register(pluginContext.Assembly, pluginId);

            HttpServerContext.Log.Debug
            (
                I18N("webexpress:internationalizationmanager.register", pluginId)
            );
        }

        /// <summary>
        /// Discovers and registers entries from the specified plugin.
        /// </summary>
        /// <param name="pluginContexts">A list with plugin contexts that contain the elements.</param>
        public void Register(IEnumerable<IPluginContext> pluginContexts)
        {
            foreach (var pluginContext in pluginContexts)
            {
                Register(pluginContext);
            }
        }

        /// <summary>
        /// Adds the internationalization key-value pairs from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly that contains the key-value pairs to insert.</param>
        /// <param name="pluginId">The id of the plugin to which the internationalization data will be assigned.</param>
        internal static void Register(Assembly assembly, string pluginId)
        {
            var assemblyName = assembly.GetName().Name.ToLower();
            var name = assemblyName + ".internationalization.";
            var resources = assembly.GetManifestResourceNames().Where(x => x.ToLower().Contains(name));

            foreach (var languageResource in resources)
            {
                var language = languageResource.Split('.').LastOrDefault()?.ToLower();

                if (!Dictionary.ContainsKey(language))
                {
                    Dictionary.Add(language, new InternationalizationItem());
                }

                var dictItem = Dictionary[language];

                using var stream = assembly.GetManifestResourceStream(languageResource);
                using var streamReader = new StreamReader(stream);
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    if (!line.StartsWith('#') && !string.IsNullOrWhiteSpace(line))
                    {
                        var split = line.Split('=');
                        var key = pluginId?.ToLower() + ":" + split[0]?.Trim().ToLower();

                        if (!dictItem.ContainsKey(key))
                        {
                            dictItem.Add(key, string.Join("=", split.Skip(1)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes all internationalization key-value pairs associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin containing the key-value pairs to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {

        }

        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="obj">An internationalization object that is being extended.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(II18N obj, string key)
        {
            return I18N(obj.Culture, key);
        }

        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="request">The request with the language to use.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(Request request, string key)
        {
            return I18N(request.Culture, null, key);
        }

        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="culture">The culture with the language to use.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(CultureInfo culture, string key)
        {
            return I18N(culture, null, key);
        }

        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="culture">The culture with the language to use.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(CultureInfo culture, string pluginId, string key)
        {
            var language = culture?.TwoLetterISOLanguageName;
            var k = string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(pluginId) || key.StartsWith($"{pluginId?.ToLower()}:") ? key?.ToLower() : $"{pluginId?.ToLower()}:{key?.ToLower()}";

            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(language) || language == "*")
            {
                language = DefaultCulture?.TwoLetterISOLanguageName;
            }

            var item = Dictionary[language];

            if (item.ContainsKey(k))
            {
                return item[k];
            }

            return key;
        }

        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="key">The internationalization key.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(string key)
        {
            return I18N(DefaultCulture, null, key);
        }

        /// <summary>
        /// Internationalization of a key.
        /// </summary>
        /// <param name="key">The internationalization key.</param>
        /// <param name="args">The formatting arguments.</param>
        /// <returns>The value of the key in the current language.</returns>
        public static string I18N(string key, params object[] args)
        {
            return string.Format(I18N(DefaultCulture, null, key), args);
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {

        }
    }
}
