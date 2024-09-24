using WebExpress.WebCore.Test.Fixture;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the plugin manager.
    /// </summary>
    /// <param name="fixture">The fixture.</param>
    [Collection("NonParallelTests")]
    public class UnitTestPlugin(UnitTestControlFixture fixture) : IClassFixture<UnitTestControlFixture>
    {
        /// <summary>
        /// Test the register function of the plugin manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();

            // test execution
            pluginManager.Register();

            Assert.Single(pluginManager.Plugins);
            Assert.Contains("webexpress.webcore.test", pluginManager.Plugins.Select(x => x.PluginId));
        }

        /// <summary>
        /// Test the register function of the plugin manager.
        /// </summary>
        [Fact]
        public void RegisterAssembly()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();

            // test execution
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);

            Assert.Single(pluginManager.Plugins);
            Assert.Contains("webexpress.webcore.test", pluginManager.Plugins.Select(x => x.PluginId));
        }

        /// <summary>
        /// Test the event of the plugin manager.
        /// </summary>
        [Fact]
        public void RegisterEvent()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            var i = 0;
            var triggered = false;

            pluginManager.AddPlugin += (s, e) => { i++; triggered = true; };

            // test execution
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);

            Assert.Single(pluginManager.Plugins);
            Assert.Contains("webexpress.webcore.test", pluginManager.Plugins.Select(x => x.PluginId));
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
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var plugin = pluginManager.Plugins.Where(x => x.PluginId == "webexpress.webcore.test").FirstOrDefault();

            // test execution
            pluginManager.Remove(plugin);

            Assert.Empty(pluginManager.Plugins);
        }

        /// <summary>
        /// Test the event of the plugin manager.
        /// </summary>
        [Fact]
        public void RemoveEvent()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var plugin = pluginManager.Plugins.Where(x => x.PluginId == "webexpress.webcore.test").FirstOrDefault();
            var i = 1;
            var triggered = false;

            pluginManager.RemovePlugin += (s, e) => { i--; triggered = true; };

            // test execution
            pluginManager.Remove(plugin);

            Assert.Empty(pluginManager.Plugins);
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
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);

            // test execution
            var plugin = pluginManager.GetPlugin("webexpress.webcore.test");

            Assert.Equal("webexpress.webcore.test", plugin?.PluginId);
        }

        /// <summary>
        /// Test the name property of the plugin.
        /// </summary>
        [Fact]
        public void GetId()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);

            // test execution
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));

            Assert.Equal(typeof(TestPlugin).Namespace.ToLower(), plugin.PluginId);
        }

        /// <summary>
        /// Test the name property of the plugin.
        /// </summary>
        [Fact]
        public void GetName()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);

            // test execution
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));

            Assert.Equal("TestPlugin", plugin.PluginName);
        }

        /// <summary>
        /// Test the description property of the plugin.
        /// </summary>
        [Fact]
        public void GetDescription()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);

            // test execution
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));

            Assert.Equal("plugin.description", plugin.Description);
        }

        /// <summary>
        /// Test the icon property of the plugin.
        /// </summary>
        [Fact]
        public void GetIcon()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);

            // test execution
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));

            Assert.Equal("/assets/img/Logo.png", plugin.Icon);
        }

        /// <summary>
        /// Test the get plugin function of the plugin manager.
        /// </summary>
        [Fact]
        public void GetPluginByType()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);

            // test execution
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));

            Assert.Equal("webexpress.webcore.test", plugin?.PluginId);
        }

        /// <summary>
        /// Test the boot function of the plugin manager.
        /// </summary>
        [Theory]
        [InlineData("webexpress.webcore.test")]
        [InlineData("non.existent.plugin")]
        [InlineData("")]
        [InlineData(null)]
        public void Boot(string pluginId)
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var plugin = pluginManager.Plugins.Where(x => x.PluginId == pluginId).FirstOrDefault();

            // test execution
            pluginManager.Boot(plugin);

            Assert.Single(pluginManager.Plugins);
            Assert.Contains("webexpress.webcore.test", pluginManager.Plugins.Select(x => x.PluginId));
        }

        /// <summary>
        /// Test the shut down of the plugin manager.
        /// </summary>
        [Theory]
        [InlineData("webexpress.webcore.test")]
        [InlineData("non.existent.plugin")]
        [InlineData("")]
        [InlineData(null)]
        public void ShutDown(string pluginId)
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var plugin = pluginManager.Plugins.Where(x => x.PluginId == pluginId).FirstOrDefault();

            // test execution
            pluginManager.ShutDown(plugin);

            Assert.Single(pluginManager.Plugins);
            Assert.Contains("webexpress.webcore.test", pluginManager.Plugins.Select(x => x.PluginId));
        }
    }
}
