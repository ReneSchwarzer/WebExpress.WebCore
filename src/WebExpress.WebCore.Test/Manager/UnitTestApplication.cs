using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the application manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestApplication
    {
        /// <summary>
        /// Test the register function of the application manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateComponentHub();
            var pluginManager = componentHub.PluginManager as PluginManager;

            // test execution
            pluginManager.Register();

            Assert.Equal(3, componentHub.ApplicationManager.Applications.Count());
            Assert.Equal("webexpress.webcore.test.testapplicationa", componentHub.ApplicationManager.GetApplication(typeof(TestApplicationA))?.ApplicationId);
            Assert.Equal("webexpress.webcore.test.testapplicationb", componentHub.ApplicationManager.GetApplication(typeof(TestApplicationB))?.ApplicationId);
            Assert.Equal("webexpress.webcore.test.testapplicationc", componentHub.ApplicationManager.GetApplication(typeof(TestApplicationC))?.ApplicationId);
        }

        /// <summary>
        /// Test the remove function of the application manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var applicationManager = componentHub.ApplicationManager as ApplicationManager;
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            applicationManager.Remove(plugin);

            Assert.Empty(componentHub.ApplicationManager.Applications);
        }

        /// <summary>
        /// Test the name property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "webexpress.webcore.test.testapplicationa")]
        [InlineData(typeof(TestApplicationB), "webexpress.webcore.test.testapplicationb")]
        [InlineData(typeof(TestApplicationC), "webexpress.webcore.test.testapplicationc")]
        public void Id(Type applicationType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);

            // test execution
            Assert.Equal(id, application.ApplicationId);
        }

        /// <summary>
        /// Test the name property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "TestApplicationA")]
        [InlineData(typeof(TestApplicationB), "TestApplicationB")]
        [InlineData(typeof(TestApplicationC), "TestApplicationC")]
        public void Name(Type applicationType, string name)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);

            // test execution
            Assert.Equal(name, application.ApplicationName);
        }

        /// <summary>
        /// Test the description property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "application.description")]
        [InlineData(typeof(TestApplicationB), "application.description")]
        [InlineData(typeof(TestApplicationC), "application.description")]
        public void Description(Type applicationType, string description)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);

            // test execution
            Assert.Equal(description, application.Description);
        }

        /// <summary>
        /// Test the icon property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/aca/assets/img/Logo.png")]
        [InlineData(typeof(TestApplicationB), "/acb/assets/img/Logo.png")]
        [InlineData(typeof(TestApplicationC), "/assets/img/Logo.png")]
        public void Icon(Type applicationType, string icon)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);

            // test execution
            Assert.Equal(icon, application.Icon);
        }

        /// <summary>
        /// Test the context path property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/aca")]
        [InlineData(typeof(TestApplicationB), "/acb")]
        [InlineData(typeof(TestApplicationC), "/")]
        public void ContextPath(Type applicationType, string contextPath)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);

            // test execution
            Assert.Equal(contextPath, application.ContextPath);
        }

        /// <summary>
        /// Test the asset path property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/aaa")]
        [InlineData(typeof(TestApplicationB), "/aab")]
        [InlineData(typeof(TestApplicationC), "/")]
        public void AssetPath(Type applicationType, string assetPath)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);

            // test execution
            Assert.Equal(assetPath, application.AssetPath);
        }

        /// <summary>
        /// Test the data path property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/ada")]
        [InlineData(typeof(TestApplicationB), "/adb")]
        [InlineData(typeof(TestApplicationC), "/")]
        public void DataPath(Type applicationType, string dataPath)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);

            // test execution
            Assert.Equal(dataPath, application.DataPath);
        }

        /// <summary>
        /// Tests whether the application manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.ApplicationManager.GetType()));
        }

        /// <summary>
        /// Tests whether the application context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            foreach (var application in componentHub.ApplicationManager.Applications)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(application.GetType()), $"Application context {application.GetType().Name} does not implement IContext.");
            }
        }
    }
}
