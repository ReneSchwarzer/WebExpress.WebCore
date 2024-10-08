using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the resource manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestResourceManager
    {
        /// <summary>
        /// Test the register function of the resource manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            // resources (3 unique + 2 ambiguous) 
            Assert.Equal(5, componentHub.ResourceManager.Resources.Count());
        }

        /// <summary>
        /// Test the remove function of the resource manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var resourceManager = componentHub.ResourceManager as ResourceManager;

            // test execution
            resourceManager.Remove(plugin);

            Assert.Empty(componentHub.ResourceManager.Resources);
        }

        /// <summary>
        /// Test the id property of the resource.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestResourceA1X), "webexpress.webcore.test.testresourcea1x")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestResourceA1Y), "webexpress.webcore.test.testresourcea1y")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), typeof(TestResourceA2X), "webexpress.webcore.test.testresourcea2x")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleAB1), typeof(TestResourceAB1X), "webexpress.webcore.test.testresourceab1x")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleAB1), typeof(TestResourceAB1X), "webexpress.webcore.test.testresourceab1x")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleAB1), typeof(TestPageA1X), null)]

        public void Id(Type applicationType, Type moduleType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentHub.ModuleManager.GetModule(applicationType, moduleType);
            var resource = componentHub.ResourceManager.GetResorce(module, resourceType);

            // test execution
            Assert.Equal(id, resource?.EndpointId);
        }

        /// <summary>
        /// Test the context path property of the resource.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestResourceA1X), "/aca/mca")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestResourceA1Y), "/aca/mca/a1x")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), typeof(TestResourceA2X), "/aca")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleAB1), typeof(TestResourceAB1X), "/aca/mcab")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleAB1), typeof(TestResourceAB1X), "/acb/mcab")]

        public void ContextPath(Type applicationType, Type moduleType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentHub.ModuleManager.GetModule(applicationType, moduleType);
            var resource = componentHub.ResourceManager.GetResorce(module, resourceType);

            // test execution
            Assert.Equal(id, resource.ContextPath);
        }

        /// <summary>
        /// Tests whether the resource manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.ResourceManager.GetType()));
        }

        /// <summary>
        /// Tests whether the resource context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            foreach (var application in componentHub.ResourceManager.Resources)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(application.GetType()), $"Resource context {application.GetType().Name} does not implement IContext.");
            }
        }
    }
}
