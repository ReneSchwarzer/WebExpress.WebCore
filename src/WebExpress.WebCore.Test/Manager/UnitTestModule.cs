using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;

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
            var componentManager = UnitTestControlFixture.CreateComponentHub();
            var pluginManager = componentManager.PluginManager as PluginManager;

            // test execution
            pluginManager.Register();

            Assert.Equal(7, componentManager.ModuleManager.Modules.Count());
        }

        /// <summary>
        /// Test the remove function of the module manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var moduleManager = componentManager.ModuleManager as ModuleManager;
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            moduleManager.Remove(plugin);

            Assert.Empty(componentManager.ModuleManager.Modules);
        }

        /// <summary>
        /// Test the id property of the module.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), "webexpress.webcore.test.testmodulea1")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), "webexpress.webcore.test.testmodulea2")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleB1), "webexpress.webcore.test.testmoduleb1")]
        public void Id(Type applicationType, Type moduleType, string id)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentHub();
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
        [InlineData(typeof(TestApplicationB), typeof(TestModuleB1), "testmoduleb1")]
        public void Name(Type applicationType, Type moduleType, string name)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentHub();
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
        [InlineData(typeof(TestApplicationB), typeof(TestModuleB1), "")]
        public void Description(Type applicationType, Type moduleType, string description)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            Assert.Equal(description, module.Description);
        }

        /// <summary>
        /// Test the icon property of the module.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), "/aca/mca/assets/img/Logo.png")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), "/aca/assets/img/Logo.png")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleB1), "/acb/mcb")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleAB1), "/aca/mcab")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleAB1), "/acb/mcab")]
        public void Icon(Type applicationType, Type moduleType, string icon)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            // test execution
            Assert.Equal(icon, module.Icon);
        }

        /// <summary>
        /// Test the context path property of the module.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), "/aca/mca")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), "/aca")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleB1), "/acb/mcb")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleAB1), "/aca/mcab")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleAB1), "/acb/mcab")]
        [InlineData(typeof(TestApplicationC), typeof(TestModuleC1), "/mcc")]
        [InlineData(typeof(TestApplicationC), typeof(TestModuleC2), "/")]
        public void ContextPath(Type applicationType, Type moduleType, string contextPath)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            // test execution
            Assert.Equal(contextPath, module.ContextPath);
        }

        /// <summary>
        /// Test the asset path property of the module.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), "/maa")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), "/aaa")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleB1), "/mab")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleAB1), "/maab")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleAB1), "/maab")]
        [InlineData(typeof(TestApplicationC), typeof(TestModuleC1), "/mac")]
        [InlineData(typeof(TestApplicationC), typeof(TestModuleC2), "/")]
        public void AssetPath(Type applicationType, Type moduleType, string assetPath)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            // test execution
            Assert.Equal(assetPath, module.AssetPath);
        }

        /// <summary>
        /// Test the data path property of the module.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), "/mda")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), "/ada")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleB1), "/mdb")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleAB1), "/mdab")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleAB1), "/mdab")]
        [InlineData(typeof(TestApplicationC), typeof(TestModuleC1), "/mdc")]
        [InlineData(typeof(TestApplicationC), typeof(TestModuleC2), "/")]
        public void DataPath(Type applicationType, Type moduleType, string dataPath)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentManager.ModuleManager.GetModule(applicationType, moduleType);

            // test execution
            Assert.Equal(dataPath, module.DataPath);
        }

        /// <summary>
        /// Tests whether the module manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentManager.ModuleManager.GetType()));
        }

        /// <summary>
        /// Tests whether the module context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            foreach (var module in componentManager.ModuleManager.Modules)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(module.GetType()), $"Module context {module.GetType().Name} does not implement IContext.");
            }
        }
    }
}
