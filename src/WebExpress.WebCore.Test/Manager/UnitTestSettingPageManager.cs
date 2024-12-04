using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the setting page manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestSettingPageManager
    {
        /// <summary>
        /// Test the register function of the setting page manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(6, componentHub.SettingPageManager.SettingPages.Count());
        }

        /// <summary>
        /// Test the remove function of the setting page manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var settingPageManager = componentHub.SettingPageManager as SettingPageManager;

            // test execution
            settingPageManager.Remove(plugin);

            Assert.Empty(componentHub.SettingPageManager.SettingPages);
        }

        /// <summary>
        /// Test the id property of the setting page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageA), "webexpress.webcore.test.testsettingpagea")]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageB), "webexpress.webcore.test.testsettingpageb")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageA), "webexpress.webcore.test.testsettingpagea")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageB), "webexpress.webcore.test.testsettingpageb")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageA), "webexpress.webcore.test.testsettingpagea")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageB), "webexpress.webcore.test.testsettingpageb")]
        public void Id(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingPage = componentHub.SettingPageManager.GetSettingPages(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, settingPage.EndpointId);
        }

        /// <summary>
        /// Test the title property of the setting page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageA), "webindex:settingpagea.label")]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageB), "webindex:settingpageb.label")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageA), "webindex:settingpagea.label")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageB), "webindex:settingpageb.label")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageA), "webindex:settingpagea.label")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageB), "webindex:settingpageb.label")]

        public void Title(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingPage = componentHub.SettingPageManager.GetSettingPages(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, settingPage.SettingPageTitle);
        }

        /// <summary>
        /// Test the context path property of the setting page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageA), "/server/appa")]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageB), "/server/appa")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageA), "/server/appb")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageB), "/server/appb")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageA), "/server")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageB), "/server")]
        public void ContextPath(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingPage = componentHub.SettingPageManager.GetSettingPages(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, settingPage.ContextPath);
        }

        /// <summary>
        /// Tests whether the setting page manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.SettingPageManager.GetType()));
        }

        /// <summary>
        /// Tests whether the setting page context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var settingPages in componentHub.SettingPageManager.SettingPages)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(settingPages.GetType()), $"Page context {settingPages.GetType().Name} does not implement IContext.");
            }
        }
    }
}
