using WebExpress.WebCore.Test.Fixture;
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
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();

            // test execution
            // resources (3 unique + 2 ambiguous) + pages (1)
            Assert.Equal(6, componentManager.ResourceManager.Resources.Count());
        }

        /// <summary>
        /// Test the remove function of the resource manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));
            var resourceManager = componentManager.ResourceManager as ResourceManager;

            // test execution
            resourceManager.Remove(plugin);

            Assert.Empty(componentManager.ResourceManager.Resources);
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

        public void Id(Type applicationType, Type moduleType, Type resourceType, string id)
        {
            // preconditions
            using var componentMoinitor = new ResourceMonitor();
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            // test execution
            var resource = componentManager.ResourceManager.GetResorce(module, resourceType);

            Assert.Equal(id, resource.ResourceId);
            Assert.False(componentMoinitor.Contains(resourceType));
        }

        /// <summary>
        /// Test the title property of the resource.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestResourceA1X), "webindex:resourcea1x.label")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestResourceA1Y), "TestResourceA1Y")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), typeof(TestResourceA2X), "webindex:resourcea2x.label")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleAB1), typeof(TestResourceAB1X), "webindex:resourceab1x.label")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleAB1), typeof(TestResourceAB1X), "webindex:resourceab1x.label")]

        public void Title(Type applicationType, Type moduleType, Type resourceType, string id)
        {
            // preconditions
            using var componentMoinitor = new ResourceMonitor();
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            // test execution
            var resource = componentManager.ResourceManager.GetResorce(module, resourceType);

            Assert.Equal(id, resource.ResourceTitle);
            Assert.False(componentMoinitor.Contains(resourceType));
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
            using var componentMoinitor = new ResourceMonitor();
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            // test execution
            var resource = componentManager.ResourceManager.GetResorce(module, resourceType);

            Assert.Equal(id, resource.ContextPath);
            Assert.False(componentMoinitor.Contains(resourceType));
        }
    }
}
