using System.Globalization;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the internationalization manager.
    /// </summary>
    /// <param name="fixture">The fixture.</param>
    [Collection("NonParallelTests")]
    public class UnitTestInternationalization(UnitTestControlFixture fixture) : IClassFixture<UnitTestControlFixture>
    {
        private static readonly object _lock = new object();

        /// <summary>
        /// Test the I18N function of the plugin manager.
        /// </summary>
        [Theory]
        [InlineData("webexpress.webcore.test.testplugin:unit.test.message", "Dies ist ein Test", "de")]
        [InlineData("webexpress.webcore.test.testplugin:unit.test.message", "This is a test", null, typeof(TestPlugin))]
        [InlineData("unit.test.message", "Dies ist ein Test", "de", typeof(TestPlugin))]
        [InlineData("unit.test.message", "Dies ist ein Test", "DE-de", typeof(TestPlugin))]
        [InlineData("unit.test.message", "This is a test", "en", typeof(TestPlugin))]
        public void I18N(string key, string excepted, string cultureName = null, Type plugin = null)
        {
            lock (_lock)
            {
                // preconditions
                fixture.RegisterPlugin(typeof(TestPlugin));
                var pluginContext = ComponentManager.PluginManager.GetPlugin(plugin);

                if (cultureName == null)
                {
                    // test execution
                    var result = InternationalizationManager.I18N(key);

                    Assert.Equal(excepted, result);
                }
                else
                {
                    // preconditions
                    var culture = CultureInfo.GetCultureInfo(cultureName);

                    // test execution
                    var result = InternationalizationManager.I18N(culture, pluginContext?.PluginId, key);

                    Assert.Equal(excepted, result);
                }
            }
        }
    }
}
