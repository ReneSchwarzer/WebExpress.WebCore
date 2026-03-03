using WebExpress.WebCore.WebIcon;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebParameter;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// Represents a test parameter.
    /// </summary>
    internal class TestParameterA : IParameterStatic
    {
        /// <summary>
        /// Returns the key that uniquely identifies the parameter in configuration or
        /// settings contexts.
        /// </summary>
        public static string Key => "testparametera";

        /// <summary>
        /// Returns or sets the scope of the parameter.
        /// </summary>
        public ParameterScope Scope { get; set; }

        /// <summary>
        /// Returns the value of the parameter.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TestParameterA()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class with a specified value.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        public TestParameterA(int value)
        {
            Value = value.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the class with a specified value.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        public TestParameterA(Guid value)
        {
            Value = value.ToString();
        }

        /// <summary>
        /// Returns a string that represents the display text for the current instance.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        /// <returns>
        /// A string containing the display text associated with the instance. The 
        /// value may be empty if no display text is available.
        /// </returns>
        public string GetDisplayText(IRenderContext renderContext)
        {
            return Value;
        }

        /// <summary>
        /// Retrieves the icon that corresponds to the specified render context.
        /// </summary>
        /// <param name="renderContext">
        /// The context in which the icon will be rendered. This parameter determines 
        /// the appearance and behavior of the
        /// returned icon.
        /// </param>
        /// <returns>
        /// An icon instance that represents the icon for the given render context.
        /// </returns>
        public IIcon GetIcon(IRenderContext renderContext)
        {
            return null;
        }

        /// <summary>
        /// Retrieves the unique key associated with the current instance.
        /// </summary>
        /// <returns>
        /// A string representing the unique key. This key is used for identifying 
        /// the instance in various operations.
        /// </returns>
        public string GetKey()
        {
            return Key;
        }
    }
}
