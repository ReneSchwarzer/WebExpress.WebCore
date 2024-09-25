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
        public void RegisterSingle()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var internationalizationManager = UnitTestControlFixture.CreateInternationalizationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            internationalizationManager.Register(plugin);

            Assert.Equal("This is a test", InternationalizationManager.I18N("webexpress.webcore.test:unit.test.message"));
        }

        /// <summary>
        /// Test the register function of the internationalization manager.
        /// </summary>
        [Fact]
        public void RegisterMultible()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var internationalizationManager = UnitTestControlFixture.CreateInternationalizationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            internationalizationManager.Register([plugin]);

            Assert.Equal("This is a test", InternationalizationManager.I18N("webexpress.webcore.test:unit.test.message"));
        }

        /// <summary>
        /// Test the remove function of the internationalization manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var internationalizationManager = UnitTestControlFixture.CreateInternationalizationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));
            internationalizationManager.Register(plugin);

            // test execution
            internationalizationManager.Remove(plugin);

            Assert.Equal("webexpress.webcore.test:unit.test.message", InternationalizationManager.I18N("webexpress.webcore.test:unit.test.message"));
        }

        /// <summary>
        /// Test the default culture property of the internationalization manager.
        /// </summary>
        [Fact]
        public void GetDefaultCulture()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var internationalizationManager = UnitTestControlFixture.CreateInternationalizationManager();

            // test execution
            Assert.Equal(CultureInfo.GetCultureInfo("en"), InternationalizationManager.DefaultCulture);
        }

        /// <summary>
        /// Test the I18N function of the plugin manager.
        /// </summary>
        [Theory]
        [InlineData("webexpress.webcore.test:unit.test.message", "This is a test")]
        [InlineData("webexpress.webcore.test:unit.test.message", "This is a test", "en")]
        [InlineData("webexpress.webcore.test:unit.test.message", "Dies ist ein Test", "de")]
        [InlineData("webexpress.webcore.test:unit.test.message", "Dies ist ein Test", "de", "webexpress.webcore.test")]
        [InlineData("webexpress.webcore.test:welcome.message", "Welcome 'Max' to our application!", "en", null, "Max")]
        [InlineData("welcome.message", "Welcome 'Max' to our application!", "en", "webexpress.webcore.test", "Max")]
        [InlineData("non.existent.key", "non.existent.key", "de")]
        public void I18N(string key, string excepted, string cultureName = null, string pluginID = null, params object[] param)
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var internationalizationManager = UnitTestControlFixture.CreateInternationalizationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));
            internationalizationManager.Register(plugin);

            if (cultureName == null && !param.Any())
            {
                // test execution
                var result = InternationalizationManager.I18N(key);

                Assert.Equal(excepted, result);
            }
            if (cultureName == null && param.Any())
            {
                // test execution
                var result = InternationalizationManager.I18N(key, param);

                Assert.Equal(excepted, result);
            }
            if (cultureName != null && pluginID == null && !param.Any())
            {
                // test execution
                var result = InternationalizationManager.I18N(CultureInfo.GetCultureInfo(cultureName), key);

                Assert.Equal(excepted, result);
            }
            if (cultureName != null && pluginID == null && param.Any())
            {
                // test execution
                var result = InternationalizationManager.I18N(CultureInfo.GetCultureInfo(cultureName), key, param);

                Assert.Equal(excepted, result);
            }
            if (cultureName != null && pluginID != null && !param.Any())
            {
                // test execution
                var result = InternationalizationManager.I18N(CultureInfo.GetCultureInfo(cultureName), pluginID, key);

                Assert.Equal(excepted, result);
            }
            if (cultureName != null && pluginID != null && param.Any())
            {
                // test execution
                var result = InternationalizationManager.I18N(CultureInfo.GetCultureInfo(cultureName), pluginID, key, param);

                Assert.Equal(excepted, result);
            }

        }
    }
}
