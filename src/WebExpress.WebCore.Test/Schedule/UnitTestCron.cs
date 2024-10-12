using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebJob;
using WebExpress.WebCore.WebLog;

namespace WebExpress.WebCore.Test.Schedule
{
    /// <summary>
    /// Test the cron job of the scheduler.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestCron
    {
        [Fact]
        public void Create_1()
        {
            // preconditions
            var context = UnitTestControlFixture.CreateHttpServerContextMock();
            var clock = new Clock();
            var cron = new Cron(context, "0-59", "*", "1-31", "1-2,3,4,5,6,7,8-10,11,12");

            // test execution
            Assert.True(cron.Matching(clock));
        }

        [Fact]
        public void Create_2()
        {
            // preconditions
            var context = UnitTestControlFixture.CreateHttpServerContextMock();
            var dateTime = DateTime.Now;
            var clock = new Clock(new DateTime(dateTime.Year, 1, dateTime.Day, dateTime.Hour, dateTime.Minute, 0));
            var cron = new Cron(context, "*", "*", "0-33", "2, 1-4, x");

            // test execution
            Assert.True(cron.Matching(clock));
        }

        [Fact]
        public void Create_3()
        {
            // preconditions
            var context = UnitTestControlFixture.CreateHttpServerContextMock();
            var dateTime = DateTime.Now;
            var clock = new Clock(new DateTime(dateTime.Year, 12, 31, dateTime.Hour, dateTime.Minute, 0));
            var cron = new Cron(context, "*", "*", "31", "12");

            // test execution
            Assert.True(cron.Matching(clock));
        }

        [Fact]
        public void Create_4()
        {
            // preconditions
            var context = UnitTestControlFixture.CreateHttpServerContextMock();
            var dateTime = DateTime.Now;
            Log.Current.Clear();

            var clock = new Clock(new DateTime(dateTime.Year, 12, 31, dateTime.Hour, dateTime.Minute, 0));
            var cron = new Cron(context, "*", "*", "*", "a");

            // test execution
            Assert.Equal(1, context.Log.WarningCount);
        }

        [Fact]
        public void Create_5()
        {
            // preconditions
            var context = UnitTestControlFixture.CreateHttpServerContextMock();
            var dateTime = DateTime.Now;
            var clock = new Clock(new DateTime(dateTime.Year, 12, 31, dateTime.Hour, dateTime.Minute, 0));
            var cron = new Cron(context, "*", "*", "*", "");

            // test execution
            Assert.True(cron.Matching(clock));
        }

        [Fact]
        public void Create_6()
        {
            // preconditions
            var context = UnitTestControlFixture.CreateHttpServerContextMock();
            var dateTime = DateTime.Now;
            Log.Current.Clear();

            var clock = new Clock(new DateTime(dateTime.Year, 12, 31, dateTime.Hour, dateTime.Minute, 0));
            var cron = new Cron(context, "99", "*", "*", "*");

            // test execution
            Assert.Equal(1, context.Log.WarningCount);
        }

        [Fact]
        public void Matching_1()
        {
            // preconditions
            var context = UnitTestControlFixture.CreateHttpServerContextMock();
            var dateTime = DateTime.Now;
            var clock = new Clock(new DateTime(dateTime.Year, 12, 31, dateTime.Hour, dateTime.Minute, 0));
            var cron = new Cron(context, "*", "*", "31", "1-11");

            // test execution
            Assert.False(cron.Matching(clock));
        }

        [Fact]
        public void Matching_2()
        {
            // preconditions
            var context = UnitTestControlFixture.CreateHttpServerContextMock();
            var dateTime = DateTime.Now;
            var clock = new Clock(new DateTime(2020, 1, 1, dateTime.Hour, dateTime.Minute, 0)); // wednesday
            var cron = new Cron(context, "*", "*", "*", "*", "3"); // wednesday

            // test execution
            Assert.True(cron.Matching(clock));
        }

        [Fact]
        public void Matching_3()
        {
            // preconditions
            var context = UnitTestControlFixture.CreateHttpServerContextMock();
            var dateTime = DateTime.Now;
            var clock = new Clock(new DateTime(2020, 1, 1, dateTime.Hour, dateTime.Minute, 0)); // wednesday
            var cron = new Cron(context, "*", "*", "*", "*", "1"); // sunday

            // test execution
            Assert.False(cron.Matching(clock));
        }
    }
}
