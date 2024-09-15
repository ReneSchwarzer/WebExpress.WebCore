using WebExpress.WebCore.Test.Fixture;

namespace WebExpress.WebCore.Test.Request
{
    [Collection("NonParallelTests")]
    public class UnitTestRequest(UnitTestControlFixture fixture) : IClassFixture<UnitTestControlFixture>
    {

        [Fact]
        public void Get_1()
        {
            Assert.True
            (
               true
            );
        }
    }
}
