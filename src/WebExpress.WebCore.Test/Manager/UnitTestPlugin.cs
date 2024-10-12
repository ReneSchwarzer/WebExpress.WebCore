using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the plugin manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestPlugin
    {
        /// <summary>
        /// Test the register function of the plugin manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateComponentHubMock();
            var pluginManager = componentHub.PluginManager as PluginManager;

            // test execution
            pluginManager.Register();

            Assert.Single(componentHub.PluginManager.Plugins);
            Assert.Contains("webexpress.webcore.test", componentHub.PluginManager.GetPlugin(typeof(TestPlugin))?.PluginId);
        }

        /// <summary>
        /// Test the event of the plugin manager.
        /// </summary>
        [Fact]
        public void RegisterEvent()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateComponentHubMock();
            var pluginManager = componentHub.PluginManager as PluginManager;
            var i = 0;
            var triggered = false;

            componentHub.PluginManager.AddPlugin += (s, e) => { i++; triggered = true; };

            // test execution
            pluginManager.Register();

            Assert.Single(componentHub.PluginManager.Plugins);
            Assert.Contains("webexpress.webcore.test", componentHub.PluginManager.GetPlugin(typeof(TestPlugin))?.PluginId);
            Assert.Equal(1, i);
            Assert.True(triggered);
        }

        /// <summary>
        /// Test the remove function of the plugin manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateComponentHubMock();
            var pluginManager = componentHub.PluginManager as PluginManager;
            pluginManager.Register();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            pluginManager.Remove(plugin);

            Assert.Empty(componentHub.PluginManager.Plugins);
        }

        /// <summary>
        /// Test the event of the plugin manager.
        /// </summary>
        [Fact]
        public void RemoveEvent()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateComponentHubMock();
            var pluginManager = componentHub.PluginManager as PluginManager;
            var i = 1;
            var triggered = false;

            componentHub.PluginManager.RemovePlugin += (s, e) => { i--; triggered = true; };
            pluginManager.Register();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            pluginManager.Remove(plugin);

            Assert.Empty(componentHub.PluginManager.Plugins);
            Assert.Equal(0, i);
            Assert.True(triggered);
        }

        /// <summary>
        /// Test the get plugin function of the plugin manager.
        /// </summary>
        [Fact]
        public void GetPluginById()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            var plugin = componentHub.PluginManager.GetPlugin("webexpress.webcore.test");

            Assert.Equal("webexpress.webcore.test", plugin?.PluginId);
        }

        /// <summary>
        /// Test the get plugin function of the plugin manager.
        /// </summary>
        [Fact]
        public void GetPluginByType()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));

            Assert.Equal("webexpress.webcore.test", plugin?.PluginId);
        }

        /// <summary>
        /// Test the name property of the plugin.
        /// </summary>
        [Fact]
        public void Id()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            Assert.Equal(typeof(TestPlugin).Namespace.ToLower(), plugin.PluginId);
        }

        /// <summary>
        /// Test the name property of the plugin.
        /// </summary>
        [Fact]
        public void GetName()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            Assert.Equal("TestPlugin", plugin.PluginName);
        }

        /// <summary>
        /// Test the description property of the plugin.
        /// </summary>
        [Fact]
        public void GetDescription()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            Assert.Equal("plugin.description", plugin.Description);
        }

        /// <summary>
        /// Test the icon property of the plugin.
        /// </summary>
        [Fact]
        public void GetIcon()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            Assert.Equal("/assets/img/Logo.png", plugin.Icon);
        }

        /// <summary>
        /// Test the boot function of the plugin manager.
        /// </summary>
        [Theory]
        [InlineData("webexpress.webcore.test", "webexpress.webcore.test")]
        [InlineData("non.existent.plugin", null)]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void Boot(string pluginId, string expected)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateComponentHubMock();
            var pluginManager = componentHub.PluginManager as PluginManager;
            pluginManager.Register();
            var plugin = componentHub.PluginManager.GetPlugin(pluginId);

            // test execution
            pluginManager.Boot(plugin);

            Assert.Single(componentHub.PluginManager.Plugins);
            Assert.Equal(expected, plugin?.PluginId);
        }

        /// <summary>
        /// Test the shut down of the plugin manager.
        /// </summary>
        [Theory]
        [InlineData("webexpress.webcore.test", "webexpress.webcore.test")]
        [InlineData("non.existent.plugin", null)]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void ShutDown(string pluginId, string expected)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateComponentHubMock();
            var pluginManager = componentHub.PluginManager as PluginManager;
            pluginManager.Register();
            var plugin = componentHub.PluginManager.GetPlugin(pluginId);

            // test execution
            pluginManager.ShutDown(plugin);

            Assert.Single(componentHub.PluginManager.Plugins);
            Assert.Equal(expected, plugin?.PluginId);
        }

        /// <summary>
        /// Tests whether the plugin manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var pluginManager = componentHub.PluginManager as PluginManager;

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.PluginManager.GetType()));
        }

        /// <summary>
        /// Tests whether the plugin context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var plugin in componentHub.PluginManager.Plugins)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(plugin.GetType()), $"Plugin context {plugin.GetType().Name} does not implement IContext.");
            }
        }
    }
}
