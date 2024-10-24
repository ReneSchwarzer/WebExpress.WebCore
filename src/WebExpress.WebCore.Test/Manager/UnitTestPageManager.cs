using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the page manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestPageManager
    {
        /// <summary>
        /// Test the register function of the page manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(9, componentHub.PageManager.Pages.Count());
        }

        /// <summary>
        /// Test the remove function of the page manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var pageManager = componentHub.PageManager as PageManager;

            // test execution
            pageManager.Remove(plugin);

            Assert.Empty(componentHub.PageManager.Pages);
        }

        /// <summary>
        /// Test the id property of the page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestPageA), "webexpress.webcore.test.testpagea")]
        [InlineData(typeof(TestApplicationA), typeof(TestPageB), "webexpress.webcore.test.testpageb")]
        [InlineData(typeof(TestApplicationA), typeof(TestPageC), "webexpress.webcore.test.testpagec")]
        [InlineData(typeof(TestApplicationB), typeof(TestPageA), "webexpress.webcore.test.testpagea")]
        [InlineData(typeof(TestApplicationB), typeof(TestPageB), "webexpress.webcore.test.testpageb")]
        [InlineData(typeof(TestApplicationB), typeof(TestPageC), "webexpress.webcore.test.testpagec")]
        [InlineData(typeof(TestApplicationC), typeof(TestPageA), "webexpress.webcore.test.testpagea")]
        [InlineData(typeof(TestApplicationC), typeof(TestPageB), "webexpress.webcore.test.testpageb")]
        [InlineData(typeof(TestApplicationC), typeof(TestPageC), "webexpress.webcore.test.testpagec")]
        public void Id(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var page = componentHub.PageManager.GetPages(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, page.EndpointId);
        }

        /// <summary>
        /// Test the title property of the page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestPageA), "webindex:pagea.label")]
        [InlineData(typeof(TestApplicationA), typeof(TestPageB), "webindex:pageb.label")]
        [InlineData(typeof(TestApplicationA), typeof(TestPageC), "webindex:pagec.label")]
        [InlineData(typeof(TestApplicationB), typeof(TestPageA), "webindex:pagea.label")]
        [InlineData(typeof(TestApplicationB), typeof(TestPageB), "webindex:pageb.label")]
        [InlineData(typeof(TestApplicationB), typeof(TestPageC), "webindex:pagec.label")]
        [InlineData(typeof(TestApplicationC), typeof(TestPageA), "webindex:pagea.label")]
        [InlineData(typeof(TestApplicationC), typeof(TestPageB), "webindex:pageb.label")]
        [InlineData(typeof(TestApplicationC), typeof(TestPageC), "webindex:pagec.label")]

        public void Title(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var page = componentHub.PageManager.GetPages(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, page.PageTitle);
        }

        /// <summary>
        /// Test the context path property of the page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestPageA), "/appa")]
        [InlineData(typeof(TestApplicationA), typeof(TestPageB), "/appa/resa")]
        [InlineData(typeof(TestApplicationA), typeof(TestPageC), "/appa")]
        [InlineData(typeof(TestApplicationB), typeof(TestPageA), "/appb")]
        [InlineData(typeof(TestApplicationB), typeof(TestPageB), "/appb/resa")]
        [InlineData(typeof(TestApplicationB), typeof(TestPageC), "/appb")]
        [InlineData(typeof(TestApplicationC), typeof(TestPageA), "/")]
        [InlineData(typeof(TestApplicationC), typeof(TestPageB), "/resa")]
        [InlineData(typeof(TestApplicationC), typeof(TestPageC), "/")]
        public void ContextPath(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var page = componentHub.PageManager.GetPages(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, page.ContextPath);
        }

        /// <summary>
        /// Tests whether the page manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.PageManager.GetType()));
        }

        /// <summary>
        /// Tests whether the page context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var pages in componentHub.PageManager.Pages)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(pages.GetType()), $"Page context {pages.GetType().Name} does not implement IContext.");
            }
        }
    }
}
