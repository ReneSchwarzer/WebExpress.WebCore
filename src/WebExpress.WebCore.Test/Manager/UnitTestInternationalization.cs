using System.Globalization;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the internationalization manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestInternationalization
    {
        /// <summary>
        /// Test the register function of the internationalization manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateComponentHub();
            var pluginManager = componentHub.PluginManager as PluginManager;

            // test execution
            pluginManager.Register();

            Assert.Equal("This is a test", I18N.Translate("webexpress.webcore.test:unit.test.message"));
        }

        /// <summary>
        /// Test the remove function of the internationalization manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var internationalizationManager = componentHub.InternationalizationManager as InternationalizationManager;
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            internationalizationManager.Remove(plugin);

            Assert.Equal("webexpress.webcore.test:unit.test.message", I18N.Translate("webexpress.webcore.test:unit.test.message"));
        }

        /// <summary>
        /// Test the default culture property of the internationalization manager.
        /// </summary>
        [Fact]
        public void GetDefaultCulture()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            Assert.Equal(CultureInfo.GetCultureInfo("en"), InternationalizationManager.DefaultCulture);
        }

        /// <summary>
        /// Test the translate function of the internationalization manager.
        /// </summary>
        [Theory]
        [InlineData("webexpress.webcore.test:unit.test.message", "This is a test")]
        [InlineData("webexpress.webcore.test:unit.test.message", "This is a test", "en")]
        [InlineData("webexpress.webcore.test:unit.test.message", "Dies ist ein Test", "de")]
        [InlineData("webexpress.webcore.test:unit.test.message", "Dies ist ein Test", "de", "webexpress.webcore.test")]
        [InlineData("webexpress.webcore.test:welcome.message", "Welcome 'Max' to our application!", "en", null, "Max")]
        [InlineData("welcome.message", "Welcome 'Max' to our application!", "en", "webexpress.webcore.test", "Max")]
        [InlineData("non.existent.key", "non.existent.key", "de")]
        public void Translate(string key, string excepted, string cultureName = null, string pluginID = null, params object[] param)
        {
            // preconditions
            UnitTestControlFixture.CreateAndRegisterComponentHub();

            if (cultureName == null && !param.Any())
            {
                // test execution
                var result = I18N.Translate(key);

                Assert.Equal(excepted, result);
            }
            if (cultureName == null && param.Any())
            {
                // test execution
                var result = I18N.Translate(key, param);

                Assert.Equal(excepted, result);
            }
            if (cultureName != null && pluginID == null && !param.Any())
            {
                // test execution
                var result = I18N.Translate(CultureInfo.GetCultureInfo(cultureName), key);

                Assert.Equal(excepted, result);
            }
            if (cultureName != null && pluginID == null && param.Any())
            {
                // test execution
                var result = I18N.Translate(CultureInfo.GetCultureInfo(cultureName), key, param);

                Assert.Equal(excepted, result);
            }
            if (cultureName != null && pluginID != null && !param.Any())
            {
                // test execution
                var result = I18N.Translate(CultureInfo.GetCultureInfo(cultureName), pluginID, key);

                Assert.Equal(excepted, result);
            }
            if (cultureName != null && pluginID != null && param.Any())
            {
                // test execution
                var result = I18N.Translate(CultureInfo.GetCultureInfo(cultureName), pluginID, key, param);

                Assert.Equal(excepted, result);
            }

        }

        /// <summary>
        /// Tests whether the internationalization manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.InternationalizationManager.GetType()));
        }
    }
}
