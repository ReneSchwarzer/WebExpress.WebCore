using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the task manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestTaskManager
    {
        /// <summary>
        /// Tests whether the task manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.ResourceManager.GetType()));
        }

        /// <summary>
        /// Tests whether the task context implements interface IComponent.
        /// </summary>
        [Fact]
        public void IsCompopnent()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            componentHub.TaskManager.CreateTask("test");

            // test execution
            foreach (var task in componentHub.TaskManager.Tasks)
            {
                Assert.True(typeof(IComponent).IsAssignableFrom(task.GetType()), $"Task {task.GetType().Name} does not implement IComponent.");
            }
        }

        /// <summary>
        /// Test the create task of the task manager.
        /// </summary>
        [Fact]
        public void CreateSystemTask()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            var task = componentHub.TaskManager.CreateTask("test");
            Assert.Equal("test", task?.Id);
        }

        /// <summary>
        /// Test the create task of the task manager.
        /// </summary>
        [Fact]
        public void CreateOwnTask()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            var task = componentHub.TaskManager.CreateTask<TestTask>("test", null, []);
            Assert.Equal("test", task?.Id);
        }

        /// <summary>
        /// Test the contains task function of the task manager.
        /// </summary>
        [Fact]
        public void ContainsTask()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var task = componentHub.TaskManager.CreateTask("test");

            // test execution
            var res = componentHub.TaskManager.ContainsTask("test");
            Assert.True(res);
        }

        /// <summary>
        /// Test the get task function of the task manager.
        /// </summary>
        [Fact]
        public void GetTask()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var task = componentHub.TaskManager.CreateTask("test");

            // test execution
            var res = componentHub.TaskManager.GetTask("test");
            Assert.Equal(task, res);
        }

        /// <summary>
        /// Test the remove task function of the task manager.
        /// </summary>
        [Fact]
        public void RemoveTask()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var task = componentHub.TaskManager.CreateTask("test");

            // test execution
            componentHub.TaskManager.RemoveTask(task);
            Assert.Empty(componentHub.TaskManager.Tasks);
        }
    }
}
