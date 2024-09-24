using System.Globalization;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.Test.Fixture;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the internationalization manager.
    /// </summary>
    /// <param name="fixture">The fixture.</param>
    [Collection("NonParallelTests")]
    public class UnitTestInternationalization(UnitTestControlFixture fixture) : IClassFixture<UnitTestControlFixture>
    {
        /// <summary>
        /// Test the register function of the internationalization manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var internationalizationManager = UnitTestControlFixture.CreateInternationalizationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            internationalizationManager.Register(plugin);
        }

        /// <summary>
        /// Test the I18N function of the plugin manager.
        /// </summary>
        [Theory]
        [InlineData("webexpress.webcore.test:unit.test.message", "Dies ist ein Test", "de")]
        [InlineData("webexpress.webcore.test:unit.test.message", "This is a test", null)]
        [InlineData("unit.test.message", "Dies ist ein Test", "de")]
        [InlineData("unit.test.message", "Dies ist ein Test", "DE-de")]
        [InlineData("unit.test.message", "This is a test", "en")]
        public void I18N(string key, string excepted, string cultureName = null)
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var internationalizationManager = UnitTestControlFixture.CreateInternationalizationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));
            internationalizationManager.Register(plugin);

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
                var result = InternationalizationManager.I18N(culture, plugin?.PluginId, key);

                Assert.Equal(excepted, result);
            }
        }
    }
}
