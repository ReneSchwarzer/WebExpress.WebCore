using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.Test.Html
{
    /// <summary>
    /// Unit tests for the DeterministicId class.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestDeterministicId
    {
        /// <summary>
        /// Tests the create method.
        /// </summary>
        [Fact]
        public void CreateSameCallsite()
        {
            // act
            var id1 = CallCreate();
            var id2 = CallCreate();

            // validation
            Assert.NotEqual(id1, id2);
        }

        /// <summary>
        /// Tests the create method.
        /// </summary>
        [Fact]
        public void CreateIndexes()
        {
            // act
            var id1 = CallCreate(0);
            var id2 = CallCreate(1);

            // validation
            Assert.NotEqual(id1, id2);
        }

        /// <summary>
        /// Tests the create method.
        /// </summary>
        [Fact]
        public void CreateDifferentCallsites()
        {
            // arrange
            var CallsiteA = new Func<string>(() => CallCreate());
            var CallsiteB = new Func<string>(() => CallCreate());

            // act
            var id1 = CallsiteA();
            var id2 = CallsiteB();

            // validation
            Assert.NotEqual(id1, id2);
        }


        /// <summary>
        /// Generates a deterministic identifier.
        /// <returns>
        /// A string that represents the generated deterministic identifier.
        /// </returns>
        private string CallCreate()
        {
            return DeterministicId.Create();
        }

        /// <summary>
        /// Generates a deterministic identifier based on the specified index.
        /// </summary>
        /// <param name="contet">
        /// The optional content used to influence the generated identifier. If 
        /// null, a default identifier is created.
        /// </param>
        /// <returns>
        /// A string that represents the generated deterministic identifier.
        /// </returns>
        private string CallCreate(object contet = null)
        {
            return DeterministicId.Create(contet);
        }
    }
}
