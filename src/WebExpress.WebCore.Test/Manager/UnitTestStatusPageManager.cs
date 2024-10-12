using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebStatusPage;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the status page manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestStatusPageManager
    {
        /// <summary>
        /// Test the register function of the status page manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(3, componentHub.StatusPageManager.StatusPages.Count());
        }

        /// <summary>
        /// Test the remove function of the status page manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var statusPageManager = componentHub.StatusPageManager as StatusPageManager;

            // test execution
            statusPageManager.Remove(plugin);

            Assert.Empty(componentHub.StatusPageManager.StatusPages);
        }

        /// <summary>
        /// Test the id property of the status page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestStatusPageA400), "webexpress.webcore.test.teststatuspagea400")]
        [InlineData(typeof(TestApplicationA), typeof(TestStatusPageA404), "webexpress.webcore.test.teststatuspagea404")]
        [InlineData(typeof(TestApplicationB), typeof(TestStatusPageB404), "webexpress.webcore.test.teststatuspageb404")]
        public void Id(Type applicationType, Type statusPageType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);
            var statusPage = componentHub.StatusPageManager.GetStatusPage(application, statusPageType);

            // test execution
            Assert.Equal(id, statusPage.StatusId);
        }

        /// <summary>
        /// Test the title property of the status page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestStatusPageA400), "webindex:homepage.label")]

        public void Title(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);
            var statusPage = componentHub.StatusPageManager.GetStatusPage(application, resourceType);

            // test execution
            Assert.Equal(id, statusPage.StatusTitle);
        }

        /// <summary>
        /// Test the id property of the status page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestStatusPageA400), 400)]
        [InlineData(typeof(TestApplicationA), typeof(TestStatusPageA404), 404)]
        [InlineData(typeof(TestApplicationB), typeof(TestStatusPageB404), 404)]
        [InlineData(typeof(TestApplicationA), typeof(TestStatusPageB500), null)]
        [InlineData(typeof(TestApplicationB), typeof(TestStatusPageB500), null)]
        public void Code(Type applicationType, Type statusPageType, int? code)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);
            var statusPage = componentHub.StatusPageManager.GetStatusPage(application, statusPageType);

            // test execution
            Assert.Equal(code, statusPage?.StatusCode);
        }

        /// <summary>
        /// Test the icon property of the status page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestStatusPageA400), "/aca/webexpress/icon.png")]
        [InlineData(typeof(TestApplicationA), typeof(TestStatusPageA404), "/aca/webexpress/icon.png")]
        [InlineData(typeof(TestApplicationB), typeof(TestStatusPageB404), null)]
        [InlineData(typeof(TestApplicationB), typeof(TestStatusPageB500), null)]
        [InlineData(typeof(TestApplicationA), typeof(TestStatusPageB500), null)]
        public void Icon(Type applicationType, Type statusPageType, string icon)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);
            var statusPage = componentHub.StatusPageManager.GetStatusPage(application, statusPageType);

            // test execution
            Assert.Equal(icon, statusPage?.StatusIcon);
        }

        /// <summary>
        /// Test the icon property of the status page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), 400, 400)]
        [InlineData(typeof(TestApplicationA), 404, 404)]
        [InlineData(typeof(TestApplicationB), 404, 404)]
        [InlineData(typeof(TestApplicationB), 500, 500)]
        [InlineData(typeof(TestApplicationA), 500, 500)]
        public void CreateAndCheckCode(Type applicationType, int statusCode, int? expected)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);
            var statusResponse = componentHub.StatusPageManager.CreateStatusResponse("content", statusCode, application, UnitTestControlFixture.CreateHttpContextMock().Request);

            // test execution
            Assert.Equal(expected, statusResponse?.Status);
        }

        /// <summary>
        /// Test the icon property of the status page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), 400, "content", "content", 78)]
        [InlineData(typeof(TestApplicationA), 500, "content", "content", 7)]
        public void CreateAndCheckMessage(Type applicationType, int statusCode, string content, string expected, int length)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplication(applicationType);
            var statusResponse = componentHub.StatusPageManager.CreateStatusResponse(content, statusCode, application, UnitTestControlFixture.CreateHttpContextMock().Request);

            // test execution
            Assert.Contains(expected, statusResponse?.Content?.ToString());
            Assert.Equal(length, statusResponse?.Header?.ContentLength);
        }

        /// <summary>
        /// Tests whether the status page manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.StatusPageManager.GetType()));
        }

        /// <summary>
        /// Tests whether the status page context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var application in componentHub.StatusPageManager.StatusPages)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(application.GetType()), $"Page context {application.GetType().Name} does not implement IContext.");
            }
        }
    }
}
