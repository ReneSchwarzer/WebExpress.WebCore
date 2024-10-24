using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebJob;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the job manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestJobManager
    {
        /// <summary>
        /// Test the register function of the job manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(3, componentHub.JobManager.Jobs.Count());
        }

        /// <summary>
        /// Test the remove function of the job manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var jobManager = componentHub.JobManager as JobManager;

            // test execution
            jobManager.Remove(plugin);

            Assert.Empty(componentHub.JobManager.Jobs);
        }

        /// <summary>
        /// Tests whether the job manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.ResourceManager.GetType()));
        }

        /// <summary>
        /// Tests whether the job context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var application in componentHub.ResourceManager.Resources)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(application.GetType()), $"Resource context {application.GetType().Name} does not implement IContext.");
            }
        }

        /// <summary>
        /// Test the id property of the job.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestJobA), "webexpress.webcore.test.testjoba")]
        [InlineData(typeof(TestApplicationB), typeof(TestJobA), "webexpress.webcore.test.testjoba")]
        [InlineData(typeof(TestApplicationC), typeof(TestJobA), "webexpress.webcore.test.testjoba")]

        public void Id(Type applicationType, Type jobType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType).FirstOrDefault();
            var job = componentHub.JobManager.GetJob(application, jobType);

            // test execution
            Assert.Equal(id, job?.JobId);
        }

        /// <summary>
        /// Test the cron property of the job.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestJobA), 50, 8, 31, new[] { 1, 2 }, 0)]
        [InlineData(typeof(TestApplicationB), typeof(TestJobA), 50, 8, 31, new[] { 1, 2 }, 0)]
        [InlineData(typeof(TestApplicationC), typeof(TestJobA), 50, 8, 31, new[] { 1, 2 }, 0)]
        public void Cron(Type applicationType, Type jobType, int minute, int hour, int day, int[] month, int weekday)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType).FirstOrDefault();
            var job = componentHub.JobManager.GetJob(application, jobType);

            // test execution
            Assert.Equal(minute, job?.Cron.Minute.FirstOrDefault() ?? -1);
            Assert.Equal(hour, job?.Cron.Hour.FirstOrDefault() ?? -1);
            Assert.Equal(day, job?.Cron.Day.FirstOrDefault() ?? -1);
            Assert.True(month.SequenceEqual(job?.Cron.Month ?? []));
            Assert.Equal(weekday, job?.Cron.Weekday.FirstOrDefault() ?? -1);
        }
    }
}
