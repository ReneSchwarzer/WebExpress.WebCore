using WebExpress.WebCore.Test.Fixture;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the module manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestModule
    {
        /// <summary>
        /// Test the register function of the module manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();

            // test execution
            componentManager.PluginManager.Register();

            Assert.Equal(2, componentManager.ModuleManager.Modules.Count());
        }

        /// <summary>
        /// Test the remove function of the module manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            componentManager.ModuleManager.Remove(plugin);

            Assert.Empty(componentManager.ModuleManager.Modules);
        }

        /// <summary>
        /// Test the id property of the module.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), "webexpress.webcore.test.testmodulea1")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), "webexpress.webcore.test.testmodulea2")]
        public void GetId(Type applicationType, Type moduleType, string id)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            Assert.Equal(id, module.ModuleId);
        }

        /// <summary>
        /// Test the name property of the module.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), "module.namea1")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), "module.namea2")]
        public void GetName(Type applicationType, Type moduleType, string name)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            Assert.Equal(name, module.ModuleName);
        }

        /// <summary>
        /// Test the description property of the module.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), "module.descriptiona1")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), "module.descriptiona2")]
        public void GetDescription(Type applicationType, Type moduleType, string description)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            Assert.Equal(description, module.Description);
        }

        /// <summary>
        /// Test the icon property of the module.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), "/assets/img/Logo.png")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), "/assets/img/Logo.png")]
        public void GetIcon(Type applicationType, Type moduleType, string icon)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            Assert.Equal(icon, module.Icon);
        }
    }
}
