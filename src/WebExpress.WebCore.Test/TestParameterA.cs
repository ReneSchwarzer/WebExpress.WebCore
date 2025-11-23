using WebExpress.WebCore.WebParameter;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// Represents a test parameter.
    /// </summary>
    internal class TestParameterA : Parameter
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TestParameterA()
            : base("TestParameterA", null, ParameterScope.Url)
        {

        }

        /// <summary>
        /// Initializes a new instance of the class with a specified value.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        public TestParameterA(int value)
            : base("TestParameterA", value, ParameterScope.Url)
        {

        }

        /// <summary>
        /// Initializes a new instance of the class with a specified value.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        public TestParameterA(Guid value)
            : base("TestParameterA", value.ToString(), ParameterScope.Url)
        {

        }
    }
}
