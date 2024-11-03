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

        ///// <summary>
        ///// Test the id property of the setting page.
        ///// </summary>
        //[Theory]
        //[InlineData(typeof(TestApplicationA), typeof(TestPageA), "webexpress.webcore.test.testpagea")]
        //[InlineData(typeof(TestApplicationA), typeof(TestPageB), "webexpress.webcore.test.testpageb")]
        //[InlineData(typeof(TestApplicationA), typeof(TestPageC), "webexpress.webcore.test.testpagec")]
        //[InlineData(typeof(TestApplicationB), typeof(TestPageA), "webexpress.webcore.test.testpagea")]
        //[InlineData(typeof(TestApplicationB), typeof(TestPageB), "webexpress.webcore.test.testpageb")]
        //[InlineData(typeof(TestApplicationB), typeof(TestPageC), "webexpress.webcore.test.testpagec")]
        //[InlineData(typeof(TestApplicationC), typeof(TestPageA), "webexpress.webcore.test.testpagea")]
        //[InlineData(typeof(TestApplicationC), typeof(TestPageB), "webexpress.webcore.test.testpageb")]
        //[InlineData(typeof(TestApplicationC), typeof(TestPageC), "webexpress.webcore.test.testpagec")]
        //public void Id(Type applicationType, Type resourceType, string id)
        //{
        //    // preconditions
        //    var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
        //    var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
        //    var settingPage = componentHub.SettingPageManager.GetPages(resourceType, application)?.FirstOrDefault();

        //    // test execution
        //    Assert.Equal(id, settingPage.EndpointId);
        //}

        ///// <summary>
        ///// Test the title property of the setting page.
        ///// </summary>
        //[Theory]
        //[InlineData(typeof(TestApplicationA), typeof(TestPageA), "webindex:pagea.label")]
        //[InlineData(typeof(TestApplicationA), typeof(TestPageB), "webindex:pageb.label")]
        //[InlineData(typeof(TestApplicationA), typeof(TestPageC), "webindex:pagec.label")]
        //[InlineData(typeof(TestApplicationB), typeof(TestPageA), "webindex:pagea.label")]
        //[InlineData(typeof(TestApplicationB), typeof(TestPageB), "webindex:pageb.label")]
        //[InlineData(typeof(TestApplicationB), typeof(TestPageC), "webindex:pagec.label")]
        //[InlineData(typeof(TestApplicationC), typeof(TestPageA), "webindex:pagea.label")]
        //[InlineData(typeof(TestApplicationC), typeof(TestPageB), "webindex:pageb.label")]
        //[InlineData(typeof(TestApplicationC), typeof(TestPageC), "webindex:pagec.label")]

        //public void Title(Type applicationType, Type resourceType, string id)
        //{
        //    // preconditions
        //    var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
        //    var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
        //    var settingPage = componentHub.SettingPageManager.GetPages(resourceType, application)?.FirstOrDefault();

        //    // test execution
        //    Assert.Equal(id, settingPage.PageTitle);
        //}

        ///// <summary>
        ///// Test the context path property of the setting page.
        ///// </summary>
        //[Theory]
        //[InlineData(typeof(TestApplicationA), typeof(TestPageA), "/appa")]
        //[InlineData(typeof(TestApplicationA), typeof(TestPageB), "/appa/resa")]
        //[InlineData(typeof(TestApplicationA), typeof(TestPageC), "/appa")]
        //[InlineData(typeof(TestApplicationB), typeof(TestPageA), "/appb")]
        //[InlineData(typeof(TestApplicationB), typeof(TestPageB), "/appb/resa")]
        //[InlineData(typeof(TestApplicationB), typeof(TestPageC), "/appb")]
        //[InlineData(typeof(TestApplicationC), typeof(TestPageA), "/")]
        //[InlineData(typeof(TestApplicationC), typeof(TestPageB), "/resa")]
        //[InlineData(typeof(TestApplicationC), typeof(TestPageC), "/")]
        //public void ContextPath(Type applicationType, Type resourceType, string id)
        //{
        //    // preconditions
        //    var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
        //    var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
        //    var settingPage = componentHub.SettingPageManager.GetPages(resourceType, application)?.FirstOrDefault();

        //    // test execution
        //    Assert.Equal(id, settingPage.ContextPath);
        //}

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
