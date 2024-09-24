using WebExpress.WebCore.Test.Fixture;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the application manager.
    /// </summary>
    /// <param name="fixture">The fixture.</param>
    [Collection("NonParallelTests")]
    public class UnitTestApplication(UnitTestControlFixture fixture) : IClassFixture<UnitTestControlFixture>
    {
        /// <summary>
        /// Test the register function of the application manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var applicationManager = UnitTestControlFixture.CreateApplicationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            applicationManager.Register(plugin);

            Assert.Equal(3, applicationManager.Applications.Count());
            Assert.Equal("webexpress.webcore.test.testapplicationa", applicationManager.GetApplcation(typeof(TestApplicationA))?.ApplicationId);
            Assert.Equal("webexpress.webcore.test.testapplicationb", applicationManager.GetApplcation(typeof(TestApplicationB))?.ApplicationId);
            Assert.Equal("testapplicationc", applicationManager.GetApplcation(typeof(TestApplicationC))?.ApplicationId);
        }

        /// <summary>
        /// Test the remove function of the application manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var applicationManager = UnitTestControlFixture.CreateApplicationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));
            applicationManager.Register(plugin);

            // test execution
            applicationManager.Remove(plugin);

            Assert.Empty(applicationManager.Applications);
        }

        /// <summary>
        /// Test the name property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "webexpress.webcore.test.testapplicationa")]
        [InlineData(typeof(TestApplicationB), "webexpress.webcore.test.testapplicationb")]
        [InlineData(typeof(TestApplicationC), "testapplicationc")]
        public void GetId(Type applicationType, string id)
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var applicationManager = UnitTestControlFixture.CreateApplicationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));
            applicationManager.Register(plugin);

            // test execution
            var applcation = applicationManager.GetApplcation(applicationType);

            Assert.Equal(id, applcation.ApplicationId);
        }

        /// <summary>
        /// Test the name property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "TestApplicationA")]
        [InlineData(typeof(TestApplicationB), "TestApplicationB")]
        [InlineData(typeof(TestApplicationC), "TestApplicationC")]
        public void GetName(Type applicationType, string name)
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var applicationManager = UnitTestControlFixture.CreateApplicationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));
            applicationManager.Register(plugin);

            // test execution
            var applcation = applicationManager.GetApplcation(applicationType);

            Assert.Equal(name, applcation.ApplicationName);
        }

        /// <summary>
        /// Test the description property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "application.description")]
        [InlineData(typeof(TestApplicationB), "application.description")]
        [InlineData(typeof(TestApplicationC), "application.description")]
        public void GetDescription(Type applicationType, string description)
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var applicationManager = UnitTestControlFixture.CreateApplicationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));
            applicationManager.Register(plugin);

            // test execution
            var applcation = applicationManager.GetApplcation(applicationType);

            Assert.Equal(description, applcation.Description);
        }

        /// <summary>
        /// Test the icon property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/assets/img/Logo.png")]
        [InlineData(typeof(TestApplicationB), "/assets/img/Logo.png")]
        [InlineData(typeof(TestApplicationC), "/assets/img/Logo.png")]
        public void GetIcon(Type applicationType, string icon)
        {
            // preconditions
            var pluginManager = UnitTestControlFixture.CreatePluginManager();
            UnitTestControlFixture.RegisterPluginManager(pluginManager, typeof(TestPlugin).Assembly);
            var applicationManager = UnitTestControlFixture.CreateApplicationManager();
            var plugin = pluginManager.GetPlugin(typeof(TestPlugin));
            applicationManager.Register(plugin);

            // test execution
            var applcation = applicationManager.GetApplcation(applicationType);

            Assert.Equal(icon, applcation.Icon);
        }
    }
}
