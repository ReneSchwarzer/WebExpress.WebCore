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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            Assert.Equal(1, componentHub.PageManager.Pages.Count());
        }

        /// <summary>
        /// Test the remove function of the page manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
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
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestPageA1X), "webexpress.webcore.test.testpagea1x")]
        public void Id(Type applicationType, Type moduleType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentHub.ModuleManager.GetModule(applicationType, moduleType);
            var page = componentHub.PageManager.GetPage(module, resourceType);

            // test execution
            Assert.Equal(id, page.EndpointId);
        }

        /// <summary>
        /// Test the title property of the page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestPageA1X), "webindex:homepage.label")]

        public void Title(Type applicationType, Type moduleType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentHub.ModuleManager.GetModule(applicationType, moduleType);
            var page = componentHub.PageManager.GetPage(module, resourceType);

            // test execution
            Assert.Equal(id, page.PageTitle);
        }

        /// <summary>
        /// Test the context path property of the page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestPageA1X), "/aca/mca")]
        public void ContextPath(Type applicationType, Type moduleType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentHub.ModuleManager.GetModule(applicationType, moduleType);
            var page = componentHub.PageManager.GetPage(module, resourceType);

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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            foreach (var application in componentHub.PageManager.Pages)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(application.GetType()), $"Page context {application.GetType().Name} does not implement IContext.");
            }
        }
    }
}
