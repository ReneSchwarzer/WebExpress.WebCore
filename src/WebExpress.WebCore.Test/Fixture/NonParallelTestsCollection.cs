namespace WebExpress.WebCore.Test.Fixture
{
    /// <summary>
    /// Defines a collection of tests that should not be run in parallel.
    /// </summary>
    [CollectionDefinition("NonParallelTests", DisableParallelization = true)]
    public class NonParallelTestsCollection : ICollectionFixture<UnitTestControlFixture>
    {

    }
}
