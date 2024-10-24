using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEvent;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the event manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestEventManager
    {
        /// <summary>
        /// Test the register function of the event manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(6, componentHub.EventManager.EventHandlers.Count());
        }

        /// <summary>
        /// Test the remove function of the event manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var eventManager = componentHub.EventManager as EventManager;
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            eventManager.Remove(plugin);

            Assert.Empty(componentHub.EventManager.EventHandlers);
        }

        /// <summary>
        /// Tests whether the event manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.EventManager.GetType()));
        }

        /// <summary>
        /// Test the id property of the event handler.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestEventA), "webexpress.webcore.test.testeventhandlera")]
        [InlineData(typeof(TestApplicationA), typeof(TestEventB), "webexpress.webcore.test.testeventhandlerb")]
        [InlineData(typeof(TestApplicationB), typeof(TestEventA), "webexpress.webcore.test.testeventhandlera")]
        [InlineData(typeof(TestApplicationB), typeof(TestEventB), "webexpress.webcore.test.testeventhandlerb")]
        public void Id(Type applicationType, Type eventType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType).FirstOrDefault();

            // test execution
            var eventHandlers = componentHub.EventManager.GetEventHandlers(application, eventType);

            if (id == null)
            {
                Assert.Empty(eventHandlers);
                return;
            }

            Assert.Contains(id, eventHandlers.Select(x => x.EventHandlerId));
        }

        /// <summary>
        /// Test the id property of the event handler.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestEventA), "webexpress.webcore.test.testeventa")]
        [InlineData(typeof(TestApplicationA), typeof(TestEventB), "webexpress.webcore.test.testeventb")]
        [InlineData(typeof(TestApplicationB), typeof(TestEventA), "webexpress.webcore.test.testeventa")]
        [InlineData(typeof(TestApplicationB), typeof(TestEventB), "webexpress.webcore.test.testeventb")]
        public void EventId(Type applicationType, Type eventType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType).FirstOrDefault();

            // test execution
            var eventHandlers = componentHub.EventManager.GetEventHandlers(application, eventType);

            if (id == null)
            {
                Assert.Empty(eventHandlers);
                return;
            }

            Assert.Contains(id, eventHandlers?.Select(x => x.EventId));
        }

        /// <summary>
        /// Test raise the event handler.
        /// </summary>
        [Fact]
        public void RaiseEventA1()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplication<TestApplicationA>();

            // test execution
            var eventArgument = new TestEventArgument() { TestProperty = false };

            componentHub.EventManager.RaiseEvent<TestEventA>(application, this, eventArgument);

            Assert.True(eventArgument.TestProperty);
        }

        /// <summary>
        /// Test raise the event handler.
        /// </summary>
        [Fact]
        public void RaiseEventB1()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplication<TestApplicationB>();

            // test execution
            var eventArgument = new TestEventArgument() { TestProperty = false };

            componentHub.EventManager.RaiseEvent<TestEventB>(application, this, eventArgument);

            Assert.True(eventArgument.TestProperty);
        }
    }
}
